using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.OldViewClass
{
    public class FollowUpView
    {
        public int FollowUpID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsUsed { get; set; }
    }

    public class FollowUpViewColumn
    {
        public const string FOLLOW_UP_ID = "FollowUpID";
        public const string FOLLOW_UP_NAME = "Name";
    }
}
