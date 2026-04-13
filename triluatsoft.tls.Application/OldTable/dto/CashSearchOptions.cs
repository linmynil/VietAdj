using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CashSearchOptions : PaginationInputBase
    {
        public string Description { get; set; }
        public string RefNbr { get; set; }
        public string PaymentMethod { get; set; }
        public string VoucherType { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
