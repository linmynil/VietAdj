using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.StoreProcDto
{
    public class CalcContribution
    {
        public int? ORD { get; set; }
        public string EmpName { get; set; }
        public int? EmpId { get; set; }
        public decimal? ConAMT { get; set; }
        public decimal? IncomAMT { get; set; }
        public decimal? BALAMT { get; set; }
        public string Audit { get; set; }
    }
}
