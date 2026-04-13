using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    [AutoMapFrom(typeof(BordereauxStatusDBObj))]
    public class CreateOrUpdateBorderauxStatusInput
    {

        [Required]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }

        public bool isActive { get; set; }

    }
}
