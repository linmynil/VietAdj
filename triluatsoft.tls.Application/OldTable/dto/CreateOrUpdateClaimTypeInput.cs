using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreateOrUpdateClaimTypeInput
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
