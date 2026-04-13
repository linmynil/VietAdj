using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.View
{
    public class VTimesheet
    {
        public int TID { get; set; }

        /// <summary>
        /// claim id
        /// </summary>
        [StringLength(25)]
        public string CID { get; set; }

        public bool? IsIssued { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? ProFeeGrandAMT { get; set; }

        public decimal? ExpenseAMT { get; set; }

        public decimal? TaxAMT { get; set; }

        public decimal? GrandAMT { get; set; }

        public DateTime? IssueDate { get; set; }

        public bool? IsSubmited { get; set; }

        public int? InsurerID { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        public bool? IsInvoiced { get; set; }
        
        public decimal? SumExpenseAMT { get; set; }

        public string TSCode { get; set; }
        public int TSSeqNo { get; set; }
        public string TimeSheetName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DateOfAssignment { get; set; }
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

        public List<Emp> Emps { get; set; }

        public int? TsRole { get; set; }

        public Nullable<int> AccountManagerID { get; set; }
    }

    public class Emp
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
