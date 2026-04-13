using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    /// <summary>
    /// Account Receivable copy from old project
    /// </summary>
    public class ARSearchOption: PaginationInputBase
    {
        public string ClaimID { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceCode { get; set; }
        public string InvoiceType { get; set; }
        public int? SettledType { get; set; }
        public bool? IsSettled { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? InsurerID { get; set; }
        public int? BrokerID { get; set; }  //hvtam-13042016
        public int? OfficeID { get; set; }  //hvtam-13042016  
        public bool? IsAdvInvoice { get; set; } //hvtam-01052016


    }
}
