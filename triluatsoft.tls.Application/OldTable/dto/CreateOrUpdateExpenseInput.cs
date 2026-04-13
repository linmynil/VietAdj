using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreateOrUpdateExpenseInput
    {
        public int TimeSheetID { get; set; }
        public int? ExpenseID { get; set; }
        public DateTime? InputDate { get; set; }
        public string Description { get; set; }
        public int ExpenseTypeID { get; set; }
        public decimal Amount { get; set; }
    }
}
