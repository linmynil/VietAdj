using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.OldViewClass;

namespace triluatsoft.tls.OldTable
{
    public class InvoiceAppService : tlsAppServiceBase, IInvoiceAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IRepository<Claim, string> _claimRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<Invoice> _invoiceRepo;
        private readonly IRepository<ACT_Liabilities> _ACT_LiabilitiesRepo;
        private readonly IRepository<ACT_Revenue> _ACT_RevenueRepo;
        private readonly IRepository<ACT_Transaction> _ACT_TransactionRepo;
        private readonly IRepository<Invoice_Timesheet> _invoiceTimesheetRepo;

        private readonly ICustomerAppService _customerService;
        private readonly ITimeSheetAppService _timesheetService;


        public InvoiceAppService(ISqlExecuter sqlExecuter
            , IRepository<Claim, string> claimRep
            , IRepository<Customer> customerRepo
            , IRepository<Invoice> invoiceRepo
            , IRepository<ACT_Liabilities> ACT_LiabilitiesRepo
            , IRepository<ACT_Revenue> ACT_RevenueRepo
            , IRepository<Invoice_Timesheet> invoiceTimesheetRepo
            , IRepository<ACT_Transaction> ACT_TransactionRepo
            , ICustomerAppService customerService
            , ITimeSheetAppService timesheetService)
        {
            _sqlExecuter = sqlExecuter;
            _claimRepo = claimRep;
            _customerRepo = customerRepo;
            _invoiceRepo = invoiceRepo;
            _ACT_LiabilitiesRepo = ACT_LiabilitiesRepo;
            _ACT_RevenueRepo = ACT_RevenueRepo;
            _invoiceTimesheetRepo = invoiceTimesheetRepo;
            _ACT_TransactionRepo = ACT_TransactionRepo;
            _customerService = customerService;
            _timesheetService = timesheetService;
        }
        public InvoicePagedResultDto<InvoiceView> Search(InvoiceSearchOption options)
        {
            switch (options.InvoiceType)
            {
                case "INTERIM": //Interim Invoice
                    options.IsAdvInvoice = true;
                    break;
                case "TIMESHEET": //Normal Invoice
                    options.IsAdvInvoice = false;
                    break;
                default: //ALL 
                    options.IsAdvInvoice = null;
                    break;
            }
            List<InvoiceView> list = new List<InvoiceView>();
            var context = _sqlExecuter.GetTLSDBContext();

            var query = from iv in context.Invoices
                        join cus in context.Customers on iv.CustomerID equals cus.Id
                        join c in context.Claims on iv.ClaimID equals c.Id
                        where (options.ClaimID == null || options.ClaimID == "" || iv.ClaimID.Contains(options.ClaimID))//|| r.ClaimID.Contains(options.ClaimID))
                            && (options.InsurerID == null || options.InsurerID == 0 || iv.CustomerID == options.InsurerID)
                            && (options.BrokerID == null || c.BrokerID == options.BrokerID)
                            && (options.InvoiceCode == null || options.InvoiceCode == "" || iv.InvoiceCode.Contains(options.InvoiceCode))
                            && (options.FromDate == null || iv.InvoiceDate >= options.FromDate)      //hvtam-18122015 r.CreateDate >= options.FromDate)   
                            && (options.ToDate == null || iv.InvoiceDate <= options.ToDate)       //hvtam-18122015 r.CreateDate <= options.ToDate)
                            && (options.OfficeID == null || c.OfficeID == options.OfficeID)
                            && (options.IsAdvInvoice == null || iv.IsAdvInvoice == options.IsAdvInvoice)    //hvtam-01052016
                                                                                                            //&& (options.IsSettled == null || iv.IsSettled == options.IsSettled)    //hvtam-01052016

                        orderby iv.CreateDate descending
                        select new InvoiceView
                        {
                            InvoiceID = iv.Id,
                            ClaimID = iv.ClaimID,
                            CustomerID = iv.CustomerID.Value,
                            CustomerBrandName = cus.BrandName,
                            InvoiceCode = iv.InvoiceCode,
                            InvoiceDate = iv.InvoiceDate,
                            IsSent2Customer = iv.IsSent2Customer.Value,
                            CreateBy = iv.CreateBy.Value,
                            CreateDate = iv.CreateDate.Value,
                            UpdateBy = iv.UpdateBy.Value,
                            UpdateDate = iv.UpdateDate.Value,

                            ProFeeGrandAMT = iv.ProFeeGrandAMT,
                            ExpenseAMT = iv.ExpenseAMT,
                            RevenueAMT = iv.RevenueAMT,
                            TaxAMT = iv.TaxAMT,
                            TotalAMT = iv.TotalAMT,

                            IsAdvInvoice = iv.IsAdvInvoice,
                            AdvRemainAMT = iv.AdvRemainAMT,
                            Note = (iv.IsAdvInvoice == true) ? "FIN" : "INT",
                            CreateByName = (from e in context.Employees where e.Id == iv.CreateBy select e.Name).FirstOrDefault(),
                            UpdateByName = (from e in context.Employees where e.Id == iv.UpdateBy select e.Name).FirstOrDefault(),
                            CustomerName = (from e in context.Customers where e.Id == iv.CustomerID select e.Name).FirstOrDefault()
                        };

            var total = query.Count();
            decimal totalRevenue = query.Sum(r => r.RevenueAMT) ?? 0;
            decimal totalNetCost = query.Sum(r => r.ProFeeGrandAMT) ?? 0;
            decimal totalExpense = query.Sum(r => r.ExpenseAMT) ?? 0;

            query = query.Skip(options.PageSize * options.Page)
                .Take(options.PageSize);

            list = query.ToList();

            var result = new InvoicePagedResultDto<InvoiceView>()
            {
                TotalCount = total
                ,Items = list
                ,TotalRevenue = totalRevenue
                , TotalExpense = totalExpense
                , TotalNetCost = totalNetCost
            };
            return result;
        }

        /// <summary>
        /// create Advance/Interim invoice
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Create(CreateInvoiceInput input)
        {
            Claim claim = _claimRepo.FirstOrDefault(input.ClaimID);

            if (claim == null || claim == null) return "Claim is not exist";

            Customer cus = _customerRepo.FirstOrDefault(input.CustomerID);
            if (cus == null || cus == null) return "Insurer is not exist";

            var currUser = GetCurrentUser();
            Invoice inv = new Invoice();
            inv.InvoiceCode = input.InvoiceCode.Trim();
            inv.InvoiceDate = input.InvoiceDate;
            inv.ClaimID = input.ClaimID;
            inv.CustomerID = input.CustomerID;
            //hvtam -21122014
            decimal Netfee = input.Fee;
            decimal Expense = input.Expense;
            decimal RevenueAMT = Netfee + Expense;
            decimal TaxAMT = 0.1M * RevenueAMT;
            if (input.NonVAT)
            {
                TaxAMT = 0;
            }
            decimal TotalAMT = RevenueAMT + TaxAMT;
            if (TotalAMT <= 0)
            {
                return "Total amount is Zero";
            }
            inv.ExpenseAMT = Expense;
            inv.ProFeeGrandAMT = Netfee;
            //End hvtam-21122014 modified
            inv.TotalAMT = TotalAMT;
            inv.AdvRemainAMT = TotalAMT;
            inv.RevenueAMT = RevenueAMT;

            inv.TaxAMT = TaxAMT;
            inv.Remark = input.Remark;
            inv.CreateDate = DateTime.Now;
            inv.UpdateDate = DateTime.Now;
            inv.CreateBy = (int)currUser.EmployeeId;
            inv.UpdateBy = (int)currUser.EmployeeId;
            inv.IsAdvInvoice = true;
            inv.IsSent2Customer = true;
            _invoiceRepo.InsertAndGetId(inv);
            CurrentUnitOfWork.SaveChanges();

            //------Create ACT_Liabilities-------------
            ACT_Liabilities lia = new ACT_Liabilities();
            lia.ClaimID = inv.ClaimID;
            lia.InvoiceID = inv.Id;
            lia.CustomerID = inv.CustomerID;

            lia.TotalAMT = inv.TotalAMT;
            lia.RemainAMT = inv.TotalAMT;
            lia.IsSettled = false;
            lia.PaidAMT = 0;
            lia.CreateBy = inv.CreateBy;
            lia.CreateDate = DateTime.Now;


            lia.CurrentBalanceDebit = CConvert.ToDecimal(cus.BalanceDebit);// Just Save
            lia.CurrentBalanceCredit = CConvert.ToDecimal(cus.BalanceCredit) + inv.TotalAMT;

            lia.TranID = CConvert.ToInt(cus.TranID) + 1;

            _ACT_LiabilitiesRepo.InsertAndGetId(lia);
            CurrentUnitOfWork.SaveChanges();

            //-------Create ACT_Revenue
            ACT_Revenue rev = new ACT_Revenue();
            rev.ClaimID = inv.ClaimID;
            rev.InvoiceID = inv.Id;
            rev.CustomerID = inv.CustomerID;
            rev.RevenueAMT = inv.RevenueAMT;
            rev.CreateBy = inv.CreateBy;
            rev.CreateDate = DateTime.Now;
            rev.IsDelete = false;
            _ACT_RevenueRepo.InsertAndGetId(rev);
            CurrentUnitOfWork.SaveChanges();
            //-------Create ACT_Tax
            //-------Update To Customer Current Balance, TransactionID
            cus.TranID = lia.TranID;
            cus.BalanceDebit = lia.CurrentBalanceDebit;
            cus.BalanceCredit = lia.CurrentBalanceCredit;
            //-------Update To Claim: TotalAdvAmount
            claim.TotalAdvAmount = CConvert.ToDecimal(claim.TotalAdvAmount) + inv.TotalAMT;
            CurrentUnitOfWork.SaveChanges();
            return "Invoice has been created successfully!";

        }
        public List<InvoiceView> GetAdvanceInvoices(string ClaimID)
        {
            List<InvoiceView> list = new List<InvoiceView>();
            var context = _sqlExecuter.GetTLSDBContext();
            var query = from iv in context.Invoices
                        where iv.IsAdvInvoice == true && iv.AdvRemainAMT > 0
                          && iv.ClaimID == ClaimID
                        orderby iv.CreateDate descending
                        select new InvoiceView
                        {
                            InvoiceID = iv.Id,
                            ClaimID = iv.ClaimID,
                            CustomerID = iv.CustomerID.Value,
                            InvoiceCode = iv.InvoiceCode,
                            InvoiceDate = iv.InvoiceDate,

                            TotalAMT = iv.TotalAMT.Value,
                            AdvRemainAMT = iv.AdvRemainAMT.Value,
                            IsAdvInvoice = iv.IsAdvInvoice,

                            CreateByName = (from e in context.Employees where e.Id == iv.CreateBy select e.Name).FirstOrDefault(),
                            UpdateByName = (from e in context.Employees where e.Id == iv.UpdateBy select e.Name).FirstOrDefault(),
                            CustomerName = (from e in context.Customers where e.Id == iv.CustomerID select e.Name).FirstOrDefault()
                        };
            list = query.ToList();

            return list;
        }

        public string CreateFinalInvoice(CreateFinalInvoiceInput input)
        {
            var currUser = GetCurrentUser();

            var context = _sqlExecuter.GetTLSDBContext();

            List<TimeSheet> lstTimesheet = (from i in context.TimeSheets
                                            where input.ListTSIds.Contains(i.Id)
                                            orderby i.CreateDate ascending
                                            select i).ToList();

            if (lstTimesheet == null || lstTimesheet.Count == 0)
            {
                return (string.Format("TimeSheet not found in DB"));
            }

            //------GET CUSTOMER ID-----------------------------------
            var cus = _customerRepo.FirstOrDefault(input.CustomerID);

            if (cus == null)
            {
                return (string.Format("Customer not found in DB"));
            }

            decimal ProFeeGrandAMT = 0;
            decimal ExpenseAMT = 0;
            decimal RevenueAMT = 0;
            decimal TaxAMT = 0;
            decimal TotalAMT = 0;

            string ClaimID = string.Empty;
            foreach (TimeSheet ts in lstTimesheet)
            {
                ClaimID = ts.ClaimID;
                if (input.NonVAT)
                {
                    ts.GrandAMT = ts.GrandAMT - ts.TaxAMT;
                    ts.TaxAMT = 0;
                }
                ProFeeGrandAMT = ProFeeGrandAMT + ((ts.ProFeeGrandAMT == null) ? 0 : Convert.ToDecimal(ts.ProFeeGrandAMT));
                ExpenseAMT = ExpenseAMT + ((ts.ExpenseAMT == null) ? 0 : Convert.ToDecimal(ts.ExpenseAMT));
                RevenueAMT = (ProFeeGrandAMT + ExpenseAMT);
                TaxAMT = TaxAMT + ((ts.TaxAMT == null) ? 0 : Convert.ToDecimal(ts.TaxAMT));
                TotalAMT = TotalAMT + ((ts.GrandAMT == null) ? 0 : Convert.ToDecimal(ts.GrandAMT));
                if (ts.IsInvoiced.Value)
                {
                    return (string.Format("TimeSheet '{0}' has been Invoice.", ts.TimeSheetName));
                }
            }

            if (input.ListInvoiceIds.Count == 0)
            {
               //Without Advance invoice, Create new Invoice for timesheet
               //-----------------------------------------
               Invoice inv = new Invoice();
                // Create Invoice
                inv.InvoiceCode = input.InvoiceCode;
                inv.InvoiceDate = input.InvoiceDate;
                inv.CreateBy = (int)currUser.EmployeeId;
                inv.UpdateBy = (int)currUser.EmployeeId;

                inv.CustomerID = input.CustomerID;
                inv.ClaimID = input.ClaimID;
                inv.CreateDate = DateTime.Now;
                inv.UpdateDate = DateTime.Now;
                //--------------------------------
                inv.ProFeeGrandAMT = ProFeeGrandAMT;
                inv.ExpenseAMT = ExpenseAMT;
                inv.RevenueAMT = (ProFeeGrandAMT + ExpenseAMT);
                inv.TaxAMT = TaxAMT;
                inv.TotalAMT = TotalAMT;
                //--------------------------------
                inv.IsSent2Customer = false;
                inv.IsAdvInvoice = false;
                _invoiceRepo.InsertAndGetId(inv);
                CurrentUnitOfWork.SaveChanges();
                //------Create Invoice Detail: Invoice-Timesheet mapp table
                foreach (TimeSheet ts in lstTimesheet)
                {
                    Invoice_Timesheet invts = new Invoice_Timesheet();
                    invts.InvoiceID = inv.Id;
                    invts.TimeSheetID = ts.Id;
                    invts.CreateBy = (int)currUser.EmployeeId;
                    invts.CreateDate = DateTime.Now;
                    _invoiceTimesheetRepo.InsertAndGetId(invts);
                    CurrentUnitOfWork.SaveChanges();
                    //------Update TimeSheet
                    ts.IsInvoiced = true;
                    ts.InvoiceCode = input.InvoiceCode;
                    ts.InvoiceDate = input.InvoiceDate;
                    ts.InvoiceBy = (int)currUser.EmployeeId;
                    CurrentUnitOfWork.SaveChanges();
                }

                //------Create ACT_Liabilities-------------
                ACT_Liabilities lia = new ACT_Liabilities();
                lia.ClaimID = ClaimID;
                lia.InvoiceID = inv.Id;
                lia.CustomerID = cus.Id;

                lia.TotalAMT = inv.TotalAMT;
                lia.RemainAMT = inv.TotalAMT;
                lia.IsSettled = false;
                lia.PaidAMT = 0;
                lia.CreateBy = (int)currUser.EmployeeId;
                lia.CreateDate = DateTime.Now;

                lia.CurrentBalanceDebit = CConvert.ToDecimal(cus.BalanceDebit) - inv.TotalAMT;
                lia.CurrentBalanceCredit = CConvert.ToDecimal(cus.BalanceCredit);// Just Save
                lia.TranID = CConvert.ToInt(cus.TranID) + 1;
                _ACT_LiabilitiesRepo.InsertAndGetId(lia);
                CurrentUnitOfWork.SaveChanges();


                //-------Create ACT_Revenue
                ACT_Revenue rev = new ACT_Revenue();
                rev.ClaimID = input.ClaimID;
                rev.InvoiceID = inv.Id;
                rev.CustomerID = (int)currUser.EmployeeId;
                rev.RevenueAMT = (ProFeeGrandAMT + ExpenseAMT);
                rev.CreateBy = (int)currUser.EmployeeId;
                rev.CreateDate = DateTime.Now;
                rev.IsDelete = false;
                _ACT_RevenueRepo.InsertAndGetId(rev);
                CurrentUnitOfWork.SaveChanges();


                //-------Update To Customer Current Balance, TransactionID
                cus.TranID = lia.TranID;
                cus.BalanceDebit = lia.CurrentBalanceDebit;
                cus.BalanceCredit = lia.CurrentBalanceCredit;
                //--------------------------------
                CurrentUnitOfWork.SaveChanges();
            }
            else
            {
                //-----------------------------------------
                List<Invoice> lstInvoice = (from i in context.Invoices
                                            where input.ListInvoiceIds.Contains(i.Id)
                                            orderby i.AdvRemainAMT ascending
                                            select i).ToList();

                decimal GrandAMT = lstTimesheet.Sum(t => t.GrandAMT).Value;
                decimal AdvAMT = lstInvoice.Sum(t => t.AdvRemainAMT).Value;
                decimal RemainTSAMT = GrandAMT - AdvAMT;

                decimal TSAmount = lstTimesheet.Sum(t => t.GrandAMT).Value;
                decimal PayAmount = 0;
                //hvtam-21122014: Them phan luu gia tri mapProFeeGrandAMT, mapExpenseAMT vao table invoice_timesheet
                decimal TsProFeeGrandAMT = lstTimesheet.Sum(t => t.ProFeeGrandAMT).Value;
                decimal TsExpenseAMT = lstTimesheet.Sum(t => t.ExpenseAMT).Value;

                decimal TSRemain = TSAmount;
                decimal TSRemainProFeeGrandAMT = TsProFeeGrandAMT;
                decimal TSRemainExpenseAMT = TsExpenseAMT;
                decimal mapProFeeGrandAMT = 0;
                decimal mapExpenseAMT = 0;
                //End hvtam-21122014

                foreach (Invoice inv in lstInvoice)
                {

                    PayAmount = (TSRemain > CConvert.ToDecimal(inv.AdvRemainAMT) ? CConvert.ToDecimal(inv.AdvRemainAMT) : TSRemain);
                    mapProFeeGrandAMT = (TSRemainProFeeGrandAMT > CConvert.ToDecimal(inv.ProFeeGrandAMT) ? CConvert.ToDecimal(inv.ProFeeGrandAMT) : TSRemainProFeeGrandAMT); //hvtam-21122014
                    mapExpenseAMT = (TSRemainExpenseAMT > CConvert.ToDecimal(inv.ExpenseAMT) ? CConvert.ToDecimal(inv.ExpenseAMT) : TSRemainExpenseAMT); //hvtam-21122014
                                                                                                                                             // Map Info
                    inv.UpdateBy = (int)currUser.EmployeeId;
                    inv.UpdateDate = DateTime.Now;
                    inv.AdvRemainAMT = inv.AdvRemainAMT - PayAmount;
                    //------Create Invoice Detail (Invoice timesheet mapping)
                    foreach (TimeSheet ts in lstTimesheet)
                    {
                        Invoice_Timesheet invts = new Invoice_Timesheet();
                        invts.InvoiceID = inv.Id;
                        invts.TimeSheetID = ts.Id;
                        invts.CreateBy = (int)currUser.EmployeeId;
                        invts.Netfee = mapProFeeGrandAMT; //hvtam-21122014
                        invts.Expene = mapExpenseAMT;
                        invts.Amount = PayAmount;
                        invts.CreateDate = DateTime.Now;
                        _invoiceTimesheetRepo.InsertAndGetId(invts);
                        //------Update TimeSheet
                        ts.IsInvoiced = true;
                        CurrentUnitOfWork.SaveChanges();
                    }
                    //hvtam-25112014 Use Advance Invoice to pay for timesheet: 
                    //-------Create ACT_Transaction
                    ACT_Transaction payment = new ACT_Transaction();

                    payment.InvoiceID = inv.Id;
                    payment.CustomerID = input.CustomerID;

                    payment.PaymentCode = "PV_INT_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    payment.PaymentDate = DateTime.Now;
                    payment.PaymentMethod = PAYMENT_METHOD_DEFINE.PAYMENT_METHOD_MAP;
                    payment.PaymentAMT = PayAmount;
                    payment.PaymentType = PAYMENT_TYPE_DEFINE.PAYMENT_TYPE_MAP;

                    payment.RefCode = "ADV_PAY_INT";
                    payment.Remark = "Accury Adv for Timesheet";
                    payment.IsNonInvoice = true;

                    payment.CreateBy = (int)currUser.EmployeeId;
                    payment.CreateDate = DateTime.Now;
                    payment.CurrentBalanceDebit = CConvert.ToDecimal(cus.BalanceDebit) + payment.PaymentAMT;
                    payment.CurrentBalanceCredit = CConvert.ToDecimal(cus.BalanceCredit); // Save info
                    payment.TranID = CConvert.ToInt(cus.TranID) + 1;
                    _ACT_TransactionRepo.InsertAndGetId(payment);
                    CurrentUnitOfWork.SaveChanges();

                    //-------Update To Customer Current Balance, TransactionID
                    cus.BalanceDebit = payment.CurrentBalanceDebit;
                    cus.BalanceCredit = payment.CurrentBalanceCredit; // Just Save Info
                    cus.TranID = payment.TranID;
                    //--------------------------------
                    context.SaveChanges();

                    //-------Update To Customer Current Balance, TransactionID
                    //--------------------------------
                    TSRemain = TSRemain - PayAmount;
                    TSRemainProFeeGrandAMT = TSRemainProFeeGrandAMT - mapProFeeGrandAMT; //hvtam-21122014
                    TSRemainExpenseAMT = TSRemainExpenseAMT - mapExpenseAMT; //hvtam-21122014
                }
                if (TSRemain > 0 && !string.IsNullOrEmpty(input.InvoiceCode))
                {
                    // Create New Invoice for TSRemain
                    TotalAMT = CConvert.ToDecimal(TSRemain);
                    RevenueAMT = Math.Round(TotalAMT / 1.1M, MidpointRounding.AwayFromZero);
                    TaxAMT = Math.Round(TotalAMT - RevenueAMT, MidpointRounding.AwayFromZero);
                    if (input.NonVAT)
                    {
                        RevenueAMT = TotalAMT;
                        TaxAMT = 0;
                    }

                    //-----------------------------------------
                    Invoice inv = new Invoice();
                    // hvtam-21122014 comment: Tao invoice moi cho phan con lai
                    inv.InvoiceCode = input.InvoiceCode;
                    inv.InvoiceDate = input.InvoiceDate;
                    inv.CreateBy = (int)currUser.EmployeeId;
                    inv.UpdateBy = (int)currUser.EmployeeId;

                    inv.CustomerID = cus.Id;
                    inv.ClaimID = ClaimID;
                    inv.CreateDate = DateTime.Now;
                    inv.UpdateDate = DateTime.Now;
                    //--------------------------------
                    inv.ProFeeGrandAMT = TSRemainProFeeGrandAMT;//hvtam-21142014: inv.ProFeeGrandAMT = 0;
                    inv.ExpenseAMT = TSRemainExpenseAMT; //hvtam-21142014:inv.ExpenseAMT = 0;
                    inv.RevenueAMT = RevenueAMT;
                    inv.TaxAMT = TaxAMT;
                    inv.TotalAMT = TotalAMT;
                    //--------------------------------
                    inv.IsSent2Customer = false;
                    inv.IsAdvInvoice = false;
                    _invoiceRepo.InsertAndGetId(inv);
                    CurrentUnitOfWork.SaveChanges();

                    foreach (TimeSheet ts in lstTimesheet)
                    {
                        //------Create Invoice Detail (Invoice - Timesheet mapping)
                        Invoice_Timesheet invts = new Invoice_Timesheet();
                        invts.InvoiceID = inv.Id;
                        invts.TimeSheetID = ts.Id;

                        invts.CreateBy = (int)currUser.EmployeeId;
                        invts.CreateDate = DateTime.Now;
                        invts.Netfee = TSRemainProFeeGrandAMT; //hvtam-21122014
                        invts.Expene = TSRemainExpenseAMT;//hvtam-21122014
                        invts.Amount = TotalAMT;//hvtam-21122014
                        _invoiceTimesheetRepo.InsertAndGetId(invts);
                        CurrentUnitOfWork.SaveChanges();

                        //------Update TimeSheet
                        ts.IsInvoiced = true;
                        ts.InvoiceCode = input.InvoiceCode;
                        ts.InvoiceDate = input.InvoiceDate;
                        ts.InvoiceBy = (int)currUser.EmployeeId;
                        CurrentUnitOfWork.SaveChanges();

                    }
                    //------Create ACT_Liabilities-------------
                    ACT_Liabilities lia = new ACT_Liabilities();
                    lia.ClaimID = ClaimID;
                    lia.InvoiceID = inv.Id;
                    lia.CustomerID = cus.Id;

                    lia.TotalAMT = inv.TotalAMT;
                    lia.RemainAMT = inv.TotalAMT;
                    lia.IsSettled = false;
                    lia.PaidAMT = 0;
                    lia.CreateBy = (int)currUser.EmployeeId;
                    lia.CreateDate = DateTime.Now;

                    lia.CurrentBalanceDebit = CConvert.ToDecimal(cus.BalanceDebit) - inv.TotalAMT;
                    lia.CurrentBalanceCredit = CConvert.ToDecimal(cus.BalanceCredit);// Just Save
                    lia.TranID = CConvert.ToInt(cus.TranID) + 1;
                    _ACT_LiabilitiesRepo.InsertAndGetId(lia);
                    CurrentUnitOfWork.SaveChanges();

                    //-------Create ACT_Revenue
                    ACT_Revenue rev = new ACT_Revenue();
                    rev.ClaimID = input.ClaimID;
                    rev.InvoiceID = inv.Id;
                    rev.CustomerID = cus.Id;
                    rev.RevenueAMT = RevenueAMT;
                    rev.CreateBy = (int)currUser.EmployeeId;
                    rev.CreateDate = DateTime.Now;
                    rev.IsDelete = false;
                    _ACT_RevenueRepo.InsertAndGetId(rev);
                    CurrentUnitOfWork.SaveChanges();

                    //-------Update To Customer Current Balance, TransactionID
                    cus.TranID = lia.TranID;
                    cus.BalanceDebit = lia.CurrentBalanceDebit;
                    cus.BalanceCredit = lia.CurrentBalanceCredit;
                    context.SaveChanges();
                    //--------------------------------
                }
            }

            return "ok";

        }
        public InvoiceView GetInfo(int InvoiceID)
        {
            InvoiceView info = new InvoiceView();
            var context = _sqlExecuter.GetTLSDBContext();
            
                var query = from iv in context.Invoices
                            where iv.Id == InvoiceID
                            orderby iv.CreateDate descending
                            select new InvoiceView
                            {
                                InvoiceID = iv.Id,
                                ClaimID = iv.ClaimID,
                                CustomerID = iv.CustomerID.Value,
                                InvoiceCode = iv.InvoiceCode,

                                IsSent2Customer = iv.IsSent2Customer.Value,
                                CreateBy = iv.CreateBy.Value,
                                CreateDate = iv.CreateDate.Value,
                                UpdateBy = iv.UpdateBy.Value,
                                UpdateDate = iv.UpdateDate.Value,

                                //ProFeeAMT = iv.ProFeeAMT,
                                //DiscountAMT = iv.DiscountAMT,
                                ProFeeGrandAMT = iv.ProFeeGrandAMT,
                                ExpenseAMT = iv.ExpenseAMT,
                                RevenueAMT = iv.RevenueAMT.Value,
                                TaxAMT = iv.TaxAMT.Value,
                                TotalAMT = iv.TotalAMT.Value,

                                CreateByName = (from e in context.Employees where e.Id == iv.CreateBy select e.Name).FirstOrDefault(),
                                UpdateByName = (from e in context.Employees where e.Id == iv.UpdateBy select e.Name).FirstOrDefault(),
                                CustomerName = (from e in context.Customers where e.Id == iv.CustomerID select e.ContactName).FirstOrDefault(),
                            };
                info = query.FirstOrDefault();
                info.Customer = _customerService.GetInfo(info.CustomerID);
                info.TimeSheets = _timesheetService.GetListByInvoiceID(info.InvoiceID);
            
            return info;
        }
        public List<InvoiceView> SearchReport(InvoiceSearchOption options)
        {
            List<InvoiceView> list = new List<InvoiceView>();
            var context = _sqlExecuter.GetTLSDBContext();            
            {
                var query = from iv in context.Invoices
                            join cus in context.Customers on iv.CustomerID equals cus.Id
                            join c in context.Claims on iv.ClaimID equals c.Id
                            where (options.ClaimID == null || options.ClaimID == "" || iv.ClaimID == options.ClaimID)//|| r.ClaimID.Contains(options.ClaimID))
                                && (options.InsurerID == null || options.InsurerID == 0 || iv.CustomerID == options.InsurerID)
                            && (options.BrokerID == null || options.BrokerID == 0 || c.BrokerID == options.BrokerID)
                            && (options.InvoiceCode == "" || options.InvoiceCode == null || iv.InvoiceCode == options.InvoiceCode)
                            && (options.FromDate == null || iv.InvoiceDate >= options.FromDate)      //hvtam-18122015 r.CreateDate >= options.FromDate)   
                            && (options.ToDate == null || iv.InvoiceDate <= options.ToDate)       //hvtam-18122015 r.CreateDate <= options.ToDate)
                            && (options.OfficeID == null || c.OfficeID == options.OfficeID)
                            && (options.IsAdvInvoice == null || iv.IsAdvInvoice == options.IsAdvInvoice)    //hvtam-01052016
                            && (options.IsSettled == null || iv.IsSettled == options.IsSettled)    //hvtam-01052016
                            orderby iv.CreateDate descending
                            select new InvoiceView
                            {
                                InvoiceID = iv.Id,
                                ClaimID = iv.ClaimID,
                                CustomerID = iv.CustomerID.Value,
                                CustomerBrandName = cus.BrandName,
                                InvoiceCode = iv.InvoiceCode,
                                InvoiceDate = iv.InvoiceDate,
                                IsSent2Customer = iv.IsSent2Customer.Value,
                                CreateBy = iv.CreateBy.Value,
                                CreateDate = iv.CreateDate.Value,
                                UpdateBy = iv.UpdateBy.Value,
                                UpdateDate = iv.UpdateDate.Value,

                                ProFeeGrandAMT = iv.ProFeeGrandAMT,
                                ExpenseAMT = iv.ExpenseAMT,
                                RevenueAMT = iv.RevenueAMT,
                                TaxAMT = iv.TaxAMT,
                                TotalAMT = iv.TotalAMT,

                                IsAdvInvoice = iv.IsAdvInvoice,
                                AdvRemainAMT = iv.AdvRemainAMT,
                                Note = (iv.IsAdvInvoice == true) ? "FIN" : "INT",
                                CreateByName = (from e in context.Employees where e.Id == iv.CreateBy select e.Name).FirstOrDefault(),
                                UpdateByName = (from e in context.Employees where e.Id == iv.UpdateBy select e.Name).FirstOrDefault(),
                                CustomerName = (from e in context.Customers where e.Id == iv.CustomerID select e.Name).FirstOrDefault()
                            };
                list = query.ToList();
            }
            return list;
        }

    }
}
