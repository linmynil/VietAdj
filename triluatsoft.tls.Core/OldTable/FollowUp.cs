using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("FollowUp")]
    public class FollowUp : Entity
    {
        public FollowUp()
        {
            this.Bordereaux = new HashSet<Bordereaux>();
        }
        
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Bordereaux> Bordereaux { get; set; }
    }
}
