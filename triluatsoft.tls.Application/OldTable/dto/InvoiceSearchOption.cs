using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class InvoiceSearchOption: PaginationInputBase
    {
        public string ClaimID { get; set; }
        public int? InsurerID { get; set; }
        public int? BrokerID { get; set; }  //hvtam-13042016
        public int? OfficeID { get; set; }  //hvtam-13042016  
        public string InvoiceCode { get; set; }
        public string InvoiceType { get; set; }
        public string lang { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsAdvInvoice { get; set; } //hvtam-01052016
        public bool? IsSettled { get; set; }    //payment
    }
}
