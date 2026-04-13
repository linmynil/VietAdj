using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("Expense")]
    public class Expense: Entity
    {
        public string ClaimID { get; set; }
        public Nullable<int> TimeSheetID { get; set; }
        public Nullable<int> ExpenseTypeID { get; set; }
        public string Description { get; set; }
        public string RefNbr { get; set; }
        public string Notes { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }

        public virtual ExpenseType ExpenseType { get; set; }

    }
}
