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
    [Table("ACT_Transaction")]
    public class ACT_Transaction : Entity
    {
        [Key]
        [Column("TransactionID")]
        public override int Id { get => base.Id; set => base.Id = value; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<int> LiabilitiesID { get; set; }
        public string PaymentCode { get; set; }
        public string RefCode { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public Nullable<decimal> PaymentAMT { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string TransType { get; set; }
        public string Remark { get; set; }
        public Nullable<decimal> CurrentBalance { get; set; }
        public Nullable<int> TranID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsNonInvoice { get; set; }
        public Nullable<decimal> CurrentBalanceCredit { get; set; }
        public Nullable<decimal> CurrentBalanceDebit { get; set; }
    }
}
