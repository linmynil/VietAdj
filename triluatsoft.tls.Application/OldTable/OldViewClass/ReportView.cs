using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.OldViewClass
{
    public class ReportView
    {
        public int ReportID { get; set; }
        public string Name { get; set; }
        public bool IsUsed { get; set; }
    }

    public class ReportViewColumn
    {
        public const string REPORT_ID = "ReportID";
        public const string REPORT_NAME = "Name";
    }
}
