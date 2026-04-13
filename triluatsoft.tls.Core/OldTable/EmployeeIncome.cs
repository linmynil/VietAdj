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
    [Table("EmployeeIncome")]
    public class EmployeeIncome:Entity
    {        
        public Nullable<int> EmployeeID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        [ForeignKey("CIncomeType")]
        public Nullable<int> IncomeType { get; set; }        
        public Nullable<bool> Contribution { get; set; }
        public Nullable<decimal> IncomeAMT { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> Updateby { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public virtual CIncomeType CIncomeType { get; set; }
        public virtual Employee Employee { get; set; }
        //public virtual UserCredential UserCredential { get; set; }
    }
}
