using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class EmployeeIncomeView
    {
        public int? ID { get; set; }
        public int? EmployeeID { get; set; }
        public string UserID { get; set; }
        public DateTime? Date { get; set; }
        public int? IncomeType { get; set; }
        public bool? Contribution { get; set; }
        public decimal? IncomeAMT { get; set; }
        public string Description { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string EmployeeName { get; set; }
        public string IncomeName { get; set; }
    }
}
