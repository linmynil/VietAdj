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
    [Table("ACT_Revenue")]
    public class ACT_Revenue : Entity
    {
        [Key]
        [Column("RevenueID")]
        public override int Id {            
            get { return base.Id; }
            set { base.Id = value; }
        }
        public string ClaimID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<decimal> RevenueAMT { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
