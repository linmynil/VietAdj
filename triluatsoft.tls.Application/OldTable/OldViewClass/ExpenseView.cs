using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.OldViewClass
{
    public class ExpenseView
    {
        public int ExpenseID { get; set; }
        public int? ExpenseTypeID { get; set; }
        public string ClaimID { get; set; }
        public int TimeSheetID { get; set; }
        public string Description { get; set; }
        public string RefNbr { get; set; }
        public string Notes { get; set; }
        public decimal? Amount { get; set; }


        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? InputDate { get; set; }

        public bool IsEditable { get; set; }

        public string ExpenseTypeName { get; set; }
        public string TimeSheetName { get; set; }
        //20140918 - DongnT
        public string TSCode { get; set; }
        public int TSSeqNo { get; set; }

        public string TSID_Name
        {
            get
            {
                return string.Format("{0}-{1}", string.Format("{0}{1}", this.TSCode, TSSeqNo.ToString("0000")), TimeSheetName);
            }
        }

        public decimal Income { get { return (Amount > 0 ? Amount.GetValueOrDefault(0) : 0); } }
        public decimal Outgo { get { return Math.Abs(Amount < 0 ? Amount.GetValueOrDefault(0) : 0); } }
        public string IncomeString { get { return (Income == 0 ? string.Empty : Income.ToString()); } }
        public string OutgoString { get { return (Outgo == 0 ? string.Empty : Outgo.ToString()); } }
        public string IncomeOutgoString { get { return (Amount > 0 ? "Income" : "Outgo"); } }
    }
}
