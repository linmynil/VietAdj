using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.ClaimManagement
{
    public class VAJMailList
    {
        public List<string> toList { get; set; }
        public List<string> ccList { get; set; }        
        public VAJMailList()
        {
            toList = new List<string>();
            ccList = new List<string>();
        }
    }
}
