using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.StoreProcDto
{
    public class CalcUserContribution
    {
        public int? ORD { get; set; }
        public string TimeSheetName { get; set; }
        public decimal? C1 { get; set; }
        public DateTime? S1 { get; set; }
        public decimal? C2 { get; set; }
        public DateTime? S2 { get; set; }
        public decimal? Balance { get; set; }
        public decimal? C { get; set; }
    }
}
