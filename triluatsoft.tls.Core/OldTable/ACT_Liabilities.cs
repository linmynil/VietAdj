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
    [Table("ACT_Liabilities")]
    public class ACT_Liabilities : Entity
    {
        [Key]
        [Column("LiabilitiesID")]
        public override int Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                base.Id = value;
            }
        }
        public string ClaimID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<decimal> ProFeeGrandAMT { get; set; }
        public Nullable<decimal> ExpenseAMT { get; set; }
        public Nullable<decimal> TaxAMT { get; set; }
        public Nullable<decimal> TotalAMT { get; set; }
        public Nullable<decimal> RemainAMT { get; set; }
        public Nullable<decimal> PaidAMT { get; set; }
        public Nullable<decimal> CurrentBalance { get; set; }
        public Nullable<int> TranID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> LastPaidDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsSettled { get; set; }
        public Nullable<System.DateTime> SettlementDate { get; set; }
        public Nullable<decimal> CurrentBalanceCredit { get; set; }
        public Nullable<decimal> CurrentBalanceDebit { get; set; }
        public Nullable<decimal> RevenueAMT { get; set; }
        
    }
}
