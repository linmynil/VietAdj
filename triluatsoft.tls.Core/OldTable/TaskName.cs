using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("TaskName")]
    public class TaskName : Entity
    {
        public string Name { get; set; }
        public string JobCode { get; set; }
        public Nullable<decimal> StandardTime { get; set; }

    }
}
