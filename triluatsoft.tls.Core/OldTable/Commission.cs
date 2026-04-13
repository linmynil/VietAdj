using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("Commission")]
    public class Commission : Entity
    {
        public Commission()
        {
            this.ACT_CommissionPayment = new HashSet<ACT_CommissionPayment>();
        }

        public int CommissionID { get; set; }
        public Nullable<int> CMSPolicyID { get; set; }
        public Nullable<int> OfficerID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<bool> IsPaid { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public Nullable<decimal> CommissionAMT { get; set; }

        public virtual CommissionPolicy CommissionPolicy { get; set; }
        public virtual CustomerOfficer CustomerOfficer { get; set; }
        public virtual ICollection<ACT_CommissionPayment> ACT_CommissionPayment { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
