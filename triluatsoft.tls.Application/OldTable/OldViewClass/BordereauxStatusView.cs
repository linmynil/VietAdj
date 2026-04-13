using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.OldViewClass
{
    public class BordereauxStatusView
    {
        public string StatusID { get; set; }
        public string Name { get; set; }
        public bool IsUsed { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ClaimStatusViewColumn
    {
        public static string STATUS_ID = "StatusID";
        public static string STATUS_NAME = "Name";
    }
}
