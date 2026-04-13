using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class ReceivementSearchOption : PaginationInputBase
    {
        public string ClaimID { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceCode { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
