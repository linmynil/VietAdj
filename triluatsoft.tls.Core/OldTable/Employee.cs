using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("Employee")]
    public class Employee : Entity<int>
    {        
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string JobPosition { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<decimal> Fee { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> isAuthen { get; set; }        
    }
}
