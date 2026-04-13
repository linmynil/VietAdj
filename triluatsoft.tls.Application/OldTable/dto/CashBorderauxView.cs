using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CashBorderauxView
    {
        public int ID { get; set; }
        public int? CashID { get; set; }
        public DateTime? InputDate { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string RefNbr { get; set; }
        public string PaymentMethod { get; set; }
        public string VoucherType { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string CashCode { get; set; }
        public string PaymentMethodString { get { return PaymentMethod == "C" ? "Cash" : "TT"; } }
        public string VoucherTypeString { get { return VoucherType == "R" ? "Receipt" : "Payment"; } }
        public decimal? ReceiptAmount { get { return VoucherType == "R" ? Amount : 0; } }
        public decimal? PaymentAmount { get { return VoucherType == "P" ? Amount : 0; } }
    }
}
