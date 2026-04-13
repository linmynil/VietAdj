using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("Cash")]
    public class Cash : Entity
    {

        public Cash()
        {
            this.CashHistory = new HashSet<CashHistory>();
        }
        
        public string CashCode { get; set; }
        public string RefNbr { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string PaymentMethod { get; set; }
        public string VoucherType { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }

        public virtual ICollection<CashHistory> CashHistory { get; set; }
    }
}
