using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreateTimesheetInput
    {
        public string ClaimID { get; set; }
        public int TSSeqNo { get; set; }
        public string TimeSheetName { get; set; }
        public decimal? ExchangeRate { get; set; }
    }
}
