using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class WIPReport
    {
        public DateTime? txtFromDate { get; set; }
        public DateTime? txtToDate { get; set; }
        public string txtClaimID { get; set; }
        public int? office { get; set; }
        public string rdolisIssued { get; set; }
        public string lang { get; set; }
        public int? paymentstt { get; set; }
        public string insurer { get; set; }
        public string broker { get; set; }
        public int? invoicetype { get; set; }
        public string invoiceno { get; set; }
    }
}
