using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.OldViewClass
{
    /// <summary>
    /// copy from old project
    /// </summary>
    public class InvoiceView
    {
        public InvoiceView()
        {
            this.InvoiceID = 0;
        }

        public int InvoiceID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerBrandName { get; set; }
        public string ClaimID { get; set; }
        public string InvoiceCode { get; set; }
        public System.DateTime? InvoiceDate { get; set; }
        public System.DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public System.DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public bool IsSent2Customer { get; set; }
        public System.DateTime? SentDate { get; set; }
        public int? SentBy { get; set; }
        public decimal? CommisionAMT { get; set; }
        public decimal? ProFeeAMT { get; set; }
        public decimal? DiscountAMT { get; set; }
        public decimal? ProFeeGrandAMT { get; set; }
        public decimal? ExpenseAMT { get; set; }
        public decimal? TaxAMT { get; set; }
        public decimal? TotalAMT { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsSettled { get; set; }
        public System.DateTime? SettlementDate { get; set; }

        public bool? IsAdvInvoice { get; set; }
        public System.DateTime? AdvanceDate { get; set; }
        public string Remark { get; set; }
        public decimal? RevenueAMT { get; set; }
        public decimal? AdvRemainAMT { get; set; }

        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public string CustomerName { get; set; }

        public string Note { get; set; }

        public List<TimeSheetView> TimeSheets { get; set; }
        public CustomerView Customer { get; set; }
    }
}
