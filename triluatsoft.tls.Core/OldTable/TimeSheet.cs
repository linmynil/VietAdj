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
    [Table("TimeSheet")]
    public class TimeSheet: Entity<int>
    {
        public TimeSheet()
        {
            //this.Expenses = new HashSet<Expense>();
            //this.Invoice_Timesheet = new HashSet<Invoice_Timesheet>();
            //this.ProfessionalFees = new HashSet<ProfessionalFee>();
        }

        [Key]
        [Column("TimeSheetID")]
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
        public string TimeSheetName { get; set; }
        public Nullable<bool> IsIssued { get; set; }
        public Nullable<bool> IsDebitNote { get; set; }
        public Nullable<bool> IsInvoiced { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<decimal> ProFeeAMT { get; set; }
        public string DiscountType { get; set; }
        public Nullable<decimal> DiscountVal { get; set; }
        public Nullable<decimal> DiscountAMT { get; set; }
        public Nullable<decimal> ProFeeGrandAMT { get; set; }
        public Nullable<decimal> ExpenseAMT { get; set; }
        public Nullable<decimal> TaxAMT { get; set; }
        public Nullable<decimal> GrandAMT { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public string DebitNoteCode { get; set; }
        public Nullable<System.DateTime> DebitNoteDate { get; set; }
        public Nullable<int> DebitNoteBy { get; set; }
        public string InvoiceCode { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<int> InvoiceBy { get; set; }
        public string TSCode { get; set; }
        public Nullable<int> TSSeqNo { get; set; }
        public Nullable<decimal> InvoiceAMT { get; set; }
        public Nullable<bool> IsSubmited { get; set; }
        public Nullable<int> SubmitBy { get; set; }
        public Nullable<System.DateTime> SubmitDate { get; set; }
        public bool IsDeleted { get; set; }
        //public virtual ICollection<Expense> Expenses { get; set; }
        //public virtual ICollection<Invoice_Timesheet> Invoice_Timesheet { get; set; }
        //public virtual ICollection<ProfessionalFee> ProfessionalFees { get; set; }
        public virtual Claim Claim { get; set; }
    }
}
