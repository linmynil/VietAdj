using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    public class SecGroupView
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<CapabilityView> Capabilities { get; set; }
        public List<UserView> Users { get; set; }
    }

    public class SecGroupViewColumn
    {
        public const string GROUP_ID = "ID";
        public const string GROUP_NAME = "Name";
    }
}
