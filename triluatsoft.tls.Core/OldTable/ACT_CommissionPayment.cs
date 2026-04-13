using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("ACT_CommissionPayment")]
    public class ACT_CommissionPayment :Entity
    {
        public int TransactionID { get; set; }
        public Nullable<int> CommissionID { get; set; }
        public string PaymentCode { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public string Remark { get; set; }
        public Nullable<decimal> PaymentAMT { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<decimal> CommissionAMT { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<decimal> RemainCommissionAMT { get; set; }

        public virtual Commission Commission { get; set; }
    }
}
