using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("tblOffice")]
    public class Office : Entity
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
