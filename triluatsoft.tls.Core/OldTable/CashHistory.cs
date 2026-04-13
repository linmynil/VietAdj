using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("CashHistory")]
    public class CashHistory : Entity
    {
        
        public Nullable<int> CashID { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string RefNbr { get; set; }
        public string PaymentMethod { get; set; }
        public string VoucherType { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CashCode { get; set; }

        public virtual Cash Cash { get; set; }
    }
}
