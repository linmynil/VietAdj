using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("CommissionPolicy")]
    public class CommissionPolicy : Entity
    {
        public CommissionPolicy()
        {
            this.Commissions = new HashSet<Commission>();
        }

        public int CMSPolicyID { get; set; }
        public string CMSPolicyName { get; set; }
        public string CMSPolicyDescription { get; set; }
        public string CMSPolicyType { get; set; }
        public Nullable<decimal> CMSValue { get; set; }
        public Nullable<bool> IsUsed { get; set; }
        public Nullable<bool> isActive { get; set; }

        public virtual ICollection<Commission> Commissions { get; set; }
    }
}
