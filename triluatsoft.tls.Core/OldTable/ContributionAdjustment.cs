using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("ContributionAdjustment")]
    public class ContributionAdjustment: Entity
    {
        public int? EmployeeID { get; set; }
        public System.DateTime? AdjustDate { get; set; }
        public decimal? AdjustAMT { get; set; }
        public int? CreateBy { get; set; }
        public System.DateTime? CreateDate { get; set; }
        public string Description { get; set; }
    
        public virtual Employee Employee { get; set; }
        //public virtual Employee Employee1 { get; set; }
    }
}
