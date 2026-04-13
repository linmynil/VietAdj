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
    [Table("Invoice")]
    public class Invoice : Entity<int>
    {
        public Invoice()
        {
            //this.ACT_Liabilities = new HashSet<ACT_Liabilities>();
            //this.ACT_Revenue = new HashSet<ACT_Revenue>();
            //this.ACT_Transaction = new HashSet<ACT_Transaction>();
            //this.Commissions = new HashSet<Commission>();
            //this.Invoice_Timesheet = new HashSet<Invoice_Timesheet>();
        }

        [Key]
        [Column("InvoiceID")]
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
        
        public Nullable<int> CustomerID { get; set; }
        public string ClaimID { get; set; }
        public string InvoiceCode { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<bool> IsAdvInvoice { get; set; }
        public Nullable<decimal> ProFeeGrandAMT { get; set; }
        public Nullable<decimal> ExpenseAMT { get; set; }
        public Nullable<decimal> RevenueAMT { get; set; }
        public Nullable<decimal> TaxAMT { get; set; }
        public Nullable<decimal> TotalAMT { get; set; }
        public Nullable<decimal> AdvRemainAMT { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<bool> IsSent2Customer { get; set; }
        public Nullable<System.DateTime> SentDate { get; set; }
        public Nullable<int> SentBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsSettled { get; set; }
        public Nullable<System.DateTime> SettlementDate { get; set; }
        public Nullable<decimal> CommisionAMT { get; set; }
        public Nullable<decimal> ProFeeAMT { get; set; }
        public Nullable<decimal> DiscountAMT { get; set; }
        public Nullable<System.DateTime> AdvanceDate { get; set; }

        //public virtual ICollection<ACT_Liabilities> ACT_Liabilities { get; set; }
        //public virtual ICollection<ACT_Revenue> ACT_Revenue { get; set; }
        //public virtual ICollection<ACT_Transaction> ACT_Transaction { get; set; }
        public virtual Claim Claim { get; set; }
        //public virtual ICollection<Commission> Commissions { get; set; }
        //public virtual Customer Customer { get; set; }
        //public virtual ICollection<Invoice_Timesheet> Invoice_Timesheet { get; set; }
    }
}
