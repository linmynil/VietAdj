using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class SearchCustomerInput
    {
        public string Name { get; set; }
        public string BrandName { get; set; }
        public bool? Status { get; set; }
        public string RecordType { get; set; }
    }
}
