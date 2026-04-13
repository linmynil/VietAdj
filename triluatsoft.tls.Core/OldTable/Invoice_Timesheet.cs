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
    [Table("Invoice_Timesheet")]
    public class Invoice_Timesheet : Entity
    {
        [Key]
        [Column("InvoiceDetailID")]
        public override int Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<int> TimeSheetID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<decimal> Netfee { get; set; }
        public Nullable<decimal> Expene { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
}
