using Abp.Application.Services.Dto;
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
    public class ARAppService: tlsAppServiceBase, IARAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public ARAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }

        public ARPagedResultDto<ARView> Search(ARSearchOption options)
        {
            var list = new List<ARView>();

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

            switch (options.SettledType)
            {
                case 0: //unpaid
                    options.IsSettled = false;
                    break;
                case 1: //paid
                    options.IsSettled = true;
                    break;
                default: //ALL 
                    options.IsSettled = null;
                    break;
            }
            var context = _sqlExecuter.GetTLSDBContext();
            
                var query = from ar in context.ACT_Liabilities
                            join iv in context.Invoices on ar.InvoiceID equals iv.Id
                            join cus in context.Customers on iv.CustomerID equals cus.Id
                            join c in context.Claims on iv.ClaimID equals c.Id
                            where
                                (options.ClaimID == null || options.ClaimID == "" || ar.ClaimID.Contains(options.ClaimID))
                                && (options.InsurerID == null || iv.CustomerID == options.InsurerID)
                                && (options.BrokerID == null || c.BrokerID == options.BrokerID)
                                && (options.InvoiceCode == null || iv.InvoiceCode.Contains(options.InvoiceCode))
                                && (options.FromDate == null || iv.InvoiceDate >= options.FromDate)      //hvtam-18122015 r.CreateDate >= options.FromDate)   
                                && (options.ToDate == null || iv.InvoiceDate <= options.ToDate)       //hvtam-18122015 r.CreateDate <= options.ToDate)
                                && (options.OfficeID == null || c.OfficeID == options.OfficeID)
                                && (options.IsAdvInvoice == null || iv.IsAdvInvoice == options.IsAdvInvoice)    //hvtam-01052016
                                && (options.IsSettled == null || ((options.IsSettled == false) && (ar.RemainAMT > 0)) || ((options.IsSettled == true) && (ar.RemainAMT == 0)))
                            orderby iv.ClaimID ascending
                            select new ARView
                            {
                                LiabilitiesID = ar.Id,
                                ClaimID = ar.ClaimID,
                                InvoiceID = ar.InvoiceID.Value,
                                CustomerID = cus.Id,

                                SubAMT = iv.ProFeeGrandAMT + iv.ExpenseAMT,
                                TaxAMT = ar.TaxAMT,
                                TotalAMT = iv.TotalAMT,
                                
                                RemainAMT = ar.RemainAMT,
                                PaidAMT = ar.PaidAMT,

                                CurrentBalanceCredit = ar.CurrentBalanceCredit,
                                CurrentBalanceDebit = ar.CurrentBalanceDebit,

                                TranID = ar.TranID.Value,

                                CreateDate = ar.CreateDate.Value,
                                CreateBy = ar.CreateBy.Value,

                                LastPaidDate = ar.LastPaidDate,
                                IsDelete = ar.IsDelete.Value,
                                IsSettled = ar.IsSettled.Value,
                                SettlementDate = ar.SettlementDate,

                                InvoiceCode = iv.InvoiceCode,
                                InvoiceDate = iv.InvoiceDate,
                                CreateByName = (from e in context.Employees where e.Id == iv.CreateBy select e.Name).FirstOrDefault(),
                                CustomerName = (from e in context.Customers where e.Id == iv.CustomerID select e.Name).FirstOrDefault(),
                                CustomerBranchName = (from e in context.Customers where e.Id == iv.CustomerID select e.BrandName).FirstOrDefault()

                            };

            var total = query.Count();
            var sum = query.Sum(item => item.TotalAMT);
            var sumTax = query.Sum(item => item.TaxAMT);
            var sumSub = query.Sum(item => item.SubAMT);

            query = query.Skip(options.PageSize * options.Page)
                .Take(options.PageSize);

            list = query.ToList();

            var result = new ARPagedResultDto<ARView>()
            {
                TotalCount = total
                , Items = list
                , SumValue = sum??0
                , TotalTax = sumTax??0
                , SubTotal = sumSub??0
            };
            return result;
        }

        public List<ARView> SearchReport(InvoiceSearchOption options) {
            var list = new List<ARView>();

            var context = _sqlExecuter.GetTLSDBContext();
            {
                var query = from ar in context.ACT_Liabilities
                            join iv in context.Invoices on ar.InvoiceID equals iv.Id
                            join cus in context.Customers on iv.CustomerID equals cus.Id
                            join c in context.Claims on iv.ClaimID equals c.Id
                            where
                                (options.ClaimID == null || options.ClaimID == "" || ar.ClaimID==options.ClaimID)
                                && (options.InsurerID == null || iv.CustomerID == options.InsurerID)
                                && (options.BrokerID == null || c.BrokerID == options.BrokerID)
                                && (options.InvoiceCode == "" || options.InvoiceCode == null || iv.InvoiceCode==options.InvoiceCode)
                                && (options.FromDate == null || iv.InvoiceDate >= options.FromDate)      //hvtam-18122015 r.CreateDate >= options.FromDate)   
                                && (options.ToDate == null || iv.InvoiceDate <= options.ToDate)       //hvtam-18122015 r.CreateDate <= options.ToDate)
                                && (options.OfficeID == null || c.OfficeID == options.OfficeID)
                                && (options.IsAdvInvoice == null || iv.IsAdvInvoice == options.IsAdvInvoice)    //hvtam-01052016
                                && (options.IsSettled == null || ar.IsSettled == options.IsSettled)
                            orderby iv.ClaimID ascending
                            select new ARView
                            {
                                LiabilitiesID = ar.Id,
                                ClaimID = ar.ClaimID,
                                InvoiceID = ar.InvoiceID.Value,
                                CustomerID = cus.Id,
                                SubAMT = iv.ProFeeGrandAMT + iv.ExpenseAMT,
                                TaxAMT = iv.TaxAMT,

                                TotalAMT = ar.TotalAMT,
                                RemainAMT = ar.RemainAMT,
                                PaidAMT = ar.PaidAMT,

                                CurrentBalanceCredit = ar.CurrentBalanceCredit,
                                CurrentBalanceDebit = ar.CurrentBalanceDebit,

                                TranID = ar.TranID.Value,

                                CreateDate = ar.CreateDate.Value,
                                CreateBy = ar.CreateBy.Value,

                                LastPaidDate = ar.LastPaidDate,
                                IsDelete = ar.IsDelete.Value,
                                IsSettled = ar.IsSettled.Value,
                                SettlementDate = ar.SettlementDate,

                                InvoiceCode = iv.InvoiceCode,
                                InvoiceDate = iv.InvoiceDate,
                                CreateByName = (from e in context.Employees where e.Id == iv.CreateBy select e.Name).FirstOrDefault(),
                                CustomerName = (from e in context.Customers where e.Id == iv.CustomerID select e.Name).FirstOrDefault(),
                                CustomerBranchName = (from e in context.Customers where e.Id == iv.CustomerID select e.BrandName).FirstOrDefault()

                            };
                list = query.ToList();
            }
            return list;
        }
    }
}
