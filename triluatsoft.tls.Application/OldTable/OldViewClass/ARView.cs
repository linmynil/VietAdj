using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.OldViewClass
{
    /// <summary>
    /// Account Receivable copy from old project
    /// </summary>
    public class ARView
    {
        public int LiabilitiesID { get; set; }
        public string ClaimID { get; set; }
        public int InvoiceID { get; set; }

        /// <summary>
        /// Insured
        /// </summary>
        public int CustomerID { get; set; }
        //public decimal? ProFeeGrandAMT { get; set; }
        //public decimal? ExpenseAMT { get; set; }
        //public decimal? RevenueAMT { get; set; }
        public decimal? SubAMT { get; set; }
        public decimal? TaxAMT { get; set; }
        public decimal? TotalAMT { get; set; }
        public decimal? RemainAMT { get; set; }
        public decimal? PaidAMT { get; set; }
        public decimal? CurrentBalanceCredit { get; set; }
        public decimal? CurrentBalanceDebit { get; set; }
        public int TranID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime? LastPaidDate { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsSettled { get; set; }
        public System.DateTime? SettlementDate { get; set; }

        public string CreateByName { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        //-----------------------------------

        public string CustomerName { get; set; }

        public string CustomerBranchName { get; set; }

        #region Properties Thanh

        public int ID { get; set; }
        public string DebitNote { get; set; }
        public DateTime? DebitNoteDate { get; set; }
        public string Insured { get; set; }

        public decimal? TotalCost { get; set; }
        public int? SettledBy { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Expense { get; set; }
        public decimal? NetCost { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }

        public string InvoiceNbr { get; set; }
        #endregion
    }
}
