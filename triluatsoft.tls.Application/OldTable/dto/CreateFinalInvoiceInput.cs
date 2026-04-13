using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreateFinalInvoiceInput
    {
        public CreateFinalInvoiceInput()
        {
            this.ListInvoiceIds = new List<int>();
            this.ListTSIds = new List<int>();
        }
        public List<int> ListTSIds { get; set; }
        public List<int> ListInvoiceIds { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime InvoiceDate { get; set; }
        public bool NonVAT { get; set; }
        public int CustomerID { get; set; }
        public string ClaimID { get; set; }
    }
}
