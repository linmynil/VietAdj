using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreateOrUpdateTypeOfLossInput
    {
        [Required]
        public string Name { get; set; }

        public int? ID { get; set; }
    }
}
