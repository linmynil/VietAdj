using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("BordereauxStatus")]
    public class BordereauxStatus : Entity<string>
    {
        public string Name { get; set; }
        public Nullable<bool> isActive { get; set; }

        public virtual ICollection<Bordereaux> Bordereaux { get; set; }
    }
}
