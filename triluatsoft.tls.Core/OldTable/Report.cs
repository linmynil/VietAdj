using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("Report")]
    public class Report : Entity
    {
        public Report()
        {
            this.Bordereaux = new HashSet<Bordereaux>();
        }
        
        public string Name { get; set; }

        public virtual ICollection<Bordereaux> Bordereaux { get; set; }
    }
}
