using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("Task")]
    public class Task : Entity
    {
        public Nullable<bool> Type { get; set; }
        public Nullable<int> TaskNameID { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string ClaimID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<bool> IsCompleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public virtual TaskName TaskName { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual Employee Employee { get; set; }

    }
}
