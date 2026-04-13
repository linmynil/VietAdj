using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.OldViewClass
{
    public class TimeSheetView
    {
        public string ClaimID { get; set; }
        public int TimeSheetID { get; set; }
        public string TimeSheetName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreateBy { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }

        public DateTime UpdateDate { get; set; }
        public int UpdateBy { get; set; }
        public Nullable<bool> IsIssued { get; set; }
        public DateTime? IssueDate { get; set; }

        public string IDCreateByName
        {
            get { return string.Format("{0} - {1}", TimeSheetID, CreateByName); }
        }

        public decimal? ExchangeRate { get; set; }
        public decimal? DiscountVal { get; set; }
        public string DiscountType { get; set; }
        public decimal? DiscountAMT { get; set; }

        public decimal? ProFeeAMTUSD { get; set; } //hvtam-24042015 Total Profee in USD

        public decimal? ProFeeAMTVND { get; set; } //hvtam-16092015 ProFeeAMT-> ProFeeAMTVND
                                                   //public decimal? ProFeeGrandAMT { get; set; } hvtam-22042015: NET Profee
        public decimal? ProFeeGrandAMT { get { return (ProFeeAMTVND - DiscountAMT); } set { } }
        public decimal? ExpenseAMT { get; set; }
        //public decimal? TaxAMT { get; set; }    hvtam-22042015 
        //public decimal? TaxAMT { get { return (ProFeeGrandAMT + ExpenseAMT) * 10 / 100; } set { } }
        public decimal? TaxAMT { get; set; }
        //public decimal? GrandAMT { get; set; }  hvtam-22042015
        public decimal? GrandAMT { get { return (ProFeeGrandAMT + ExpenseAMT + TaxAMT); } set { } } //  hvtam-22042015 Math.Round(ProFeeGrandAMT + ExpenseAMT + TaxAMT, 0)????

        //hvtam-24042015: Actual value theo ActualTime
        //public decimal? ActualProFeeAMT { get; set; }
        //public decimal? ActualProFeeGrandAMT { get { return (ActualProFeeAMT - DiscountAMT); } set { } }                //hvtam-22042015 computed propery
        //public decimal? ActualTaxAMT { get { return (ActualProFeeGrandAMT + ExpenseAMT) * 10 / 100; } set { } }         //hvtam-22042015 computed propery
        //public decimal? ActualGrandAMT { get { return (ActualProFeeGrandAMT + ExpenseAMT + ActualTaxAMT); } set { } }   //hvtam-22042015 computed propery
        //endhvtam-24042015: Actual value theo ActualTime

        //hvtam-24042015
        //public decimal NetCost { get; set; }
        //public decimal EstimatedNetCost { get; set; }
        //public decimal Expense { get; set; }
        //public decimal Tax { get { return (EstimatedNetCost + Expense) * 10 / 100; } }
        //public decimal Total { get { return EstimatedNetCost + Expense + Tax; } }
        //endhvtam-24042015

        public List<ProfessionalFeeView> ListProFee { get; set; }
        public List<ExpenseView> ListExpense { get; set; }

        public bool IsInvoiced { get; set; }
        public decimal? InvoiceAMT { get; set; }
        //20140905 - DongNT Add
        public bool IsDebitNote { get; set; }
        public string DebitNoteCode { get; set; }
        public DateTime? DebitNoteDate { get; set; }
        public int DebitNoteBy { get; set; }
        public string DebitNoteByName { get; set; }
        //20140909 - DongNT 
        public string InvoiceCode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int InvoiceBy { get; set; }
        public string InvoiceByName { get; set; }
        public string CustomerName { get; set; }
        //20140918 - DongnT
        public string TSCode { get; set; }
        public int TSSeqNo { get; set; }
        public bool? IsSubmited { get; set; }
        public int? SubmitBy { get; set; }
        public string SubmitByName { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string TSID
        {
            get
            {
                return string.Format("{0}{1}", this.TSCode, TSSeqNo.ToString("0000"));
            }
        }
        public string TSID_Name
        {
            get
            {
                return string.Format("{0}-{1}", this.TSID, TimeSheetName);
            }
        }

        public decimal? ActualFeeVND { get; set; }
        //end all 

        public List<EmployeeView> ListEmpView { get; set; }
    }
}
