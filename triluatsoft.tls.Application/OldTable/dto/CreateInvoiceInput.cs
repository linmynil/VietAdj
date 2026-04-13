using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreateInvoiceInput
    {
        public string InvoiceCode { get; set;}
        public DateTime InvoiceDate { get; set; }
        public string ClaimID { get; set; }
        public int CustomerID { get; set; }
        public decimal Fee { get; set; }
        public decimal Expense { get; set; }
        public bool NonVAT { get; set; }
        public string Remark { get; set; }
    }
}
