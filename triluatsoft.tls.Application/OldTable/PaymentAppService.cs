using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.OldViewClass;
using triluatsoft.tls.EntityFramework;

namespace triluatsoft.tls.OldTable
{
    public class PaymentAppService : tlsAppServiceBase, IPaymentAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public PaymentAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }

        public PagedResultDto<CPaymentView> Search(ReceivementSearchOption opt)
        {

            var list = new List<CPaymentView>();
            var context = _sqlExecuter.GetTLSDBContext();

            var query = from t in context.ACT_Transactions
                        join iv in context.Invoices on t.InvoiceID equals iv.Id
                        join c in context.Customers on t.CustomerID equals c.Id
                        where (t.PaymentType != "M")
                            && (string.IsNullOrEmpty(opt.ClaimID) || iv.ClaimID == opt.ClaimID)
                            && (string.IsNullOrEmpty(opt.CustomerName) || c.Name.Contains(opt.CustomerName))
                            && (string.IsNullOrEmpty(opt.InvoiceCode) || iv.InvoiceCode.Contains(opt.InvoiceCode))
                            && (string.IsNullOrEmpty(opt.PaymentMethod) || t.PaymentMethod == opt.PaymentMethod)
                            && (opt.FromDate == null || t.PaymentDate >= opt.FromDate)
                            && (opt.ToDate == null || t.PaymentDate <= opt.ToDate)
                        orderby iv.CreateDate descending
                        select new CPaymentView
                        {
                            TransactionID = t.Id,
                            LiabilitiesID = t.LiabilitiesID ?? 0,
                            InvoiceID = t.InvoiceID.Value,
                            CustomerID = c.Id,
                            CurrentBalanceCredit = t.CurrentBalanceCredit ?? 0,
                            CurrentBalanceDebit = t.CurrentBalanceDebit ?? 0,
                            TranID = t.TranID.Value,

                            CreateDate = t.CreateDate.Value,
                            CreateBy = t.CreateBy.Value,

                            PaymentAMT = t.PaymentAMT ?? 0,
                            PaymentCode = t.PaymentCode,
                            PaymentDate = t.PaymentDate.Value,
                            PaymentMethod = (t.PaymentMethod == PAYMENT_METHOD_DEFINE.PAYMENT_METHOD_CASH ? "Cash" : "TT"),
                            PaymentType = t.PaymentType,

                            RefCode = t.RefCode,
                            Remark = t.Remark,

                            ClaimID = iv.ClaimID,
                            InvoiceCode = iv.InvoiceCode,
                            InvoiceDate = iv.InvoiceDate.Value,
                            CreateByName = (from e in context.Employees where e.Id == iv.CreateBy select e.Name).FirstOrDefault(),
                            CustomerName = (from e in context.Customers where e.Id == iv.CustomerID select e.Name).FirstOrDefault()

                        };

            var total = query.Count();

            query = query.Skip(opt.PageSize * opt.Page)
                .Take(opt.PageSize);

            list = query.ToList();

            var result = new PagedResultDto<CPaymentView>()
            {
                TotalCount = total
                ,
                Items = list
            };
            return result;


        }
        public string Create(CreatePaymentInput input)
        {
            var context = _sqlExecuter.GetTLSDBContext();


            var currUser = GetCurrentUser();

            ACT_Liabilities lia = context.ACT_Liabilities.Find(input.LiabilitiesID);
            if (lia == null)
            {
                Logger.Error(string.Format("Liabilities {0} not found in DB", input.LiabilitiesID));
                return "Payment liabilities not found";

            }

            if (lia.IsSettled.Value)
            {
                Logger.Error(string.Format("Liabilities '{0}' has been done.", input.LiabilitiesID));
                return "Payment liabilities has paid done";

            }
            //------GET CUSTOMER ID-----------------------------------            
            Customer cus = context.Customers.Find(lia.CustomerID);

            //-------Update To Liabialities PaidAMT, RemainAMT
            lia.PaidAMT = CConvert.ToDecimal(lia.PaidAMT) + input.PaymentAMT;
            lia.RemainAMT = CConvert.ToDecimal(lia.RemainAMT) - input.PaymentAMT;

            if (lia.RemainAMT == 0)
            {
                lia.IsSettled = true;
                lia.SettlementDate = DateTime.Now;

                //hvtam-06052016 Update Invoice flag is paid
                Invoice inv = context.Invoices.Find(lia.InvoiceID);
                inv.IsSettled = true;
                //end hvtam-06052016 Update Invoice flag is paid
            }
            else if (lia.RemainAMT < 0) //hvtam-08042015 Khong cho tra so tien lon hon phan con lai
            {
                return "Payment number is higher remain debt";
            }
            //--------------------------------
            ACT_Transaction paymenttrans = new ACT_Transaction();
            paymenttrans.LiabilitiesID = lia.Id;

            paymenttrans.InvoiceID = lia.InvoiceID;
            paymenttrans.CustomerID = lia.CustomerID;

            paymenttrans.PaymentCode = input.PaymentCode;
            paymenttrans.PaymentDate = input.PaymentDate;
            paymenttrans.PaymentMethod = input.PaymentMethod;
            paymenttrans.PaymentAMT = input.PaymentAMT;
            paymenttrans.PaymentType = PAYMENT_TYPE_DEFINE.PAYMENT_TYPE_RECEIVE; //Receivement

            paymenttrans.RefCode = input.RefCode;
            paymenttrans.Remark = input.Remark;
            paymenttrans.IsNonInvoice = false;

            paymenttrans.CreateBy = (int)currUser.EmployeeId;
            paymenttrans.CreateDate = DateTime.Now;
            paymenttrans.CurrentBalanceDebit = CConvert.ToDecimal(cus.BalanceDebit) + paymenttrans.PaymentAMT;
            paymenttrans.CurrentBalanceCredit = CConvert.ToDecimal(cus.BalanceCredit); // Save info
            paymenttrans.TranID = CConvert.ToInt(paymenttrans.TranID) + 1;
            context.ACT_Transactions.Add(paymenttrans);
            //-------Update To Customer Current Balance, TransactionID
            cus.BalanceDebit = paymenttrans.CurrentBalanceDebit;
            cus.BalanceCredit = paymenttrans.CurrentBalanceCredit; // Just Save Info
            cus.TranID = paymenttrans.TranID;
            //--------------------------------
            CurrentUnitOfWork.SaveChanges();
            return "ok";

        }
    }
}
