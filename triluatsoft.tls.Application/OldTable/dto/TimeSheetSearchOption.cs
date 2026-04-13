using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class TimeSheetSearchOption : PaginationInputBase
    {
        public string ClaimID { get; set; }
        public string TimeSheet { get; set; }
        public bool? isIssued { get; set; }
        public bool? HaveDebitNote { get; set; }
        public bool? HaveInvoice { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public int? TsRole { get; set; }
    }
}
