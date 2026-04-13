using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    public class CapabilityView
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class CapabilityViewColumn
    {
        public const string CAPABILITY_ID = "ID";
        public const string CAPABILITY_NAME = "Name";
    }
}
