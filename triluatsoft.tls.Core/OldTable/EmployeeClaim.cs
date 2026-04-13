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
    [Table("EmployeeClaim")]
    public class EmployeeClaim : Entity
    {
        public int EmployeeID { get; set; }
        public string ClaimID { get; set; }
    }
}
