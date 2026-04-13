using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.View
{
    public class CashView
    {
        public int ID { get; set; }
        public string CashCode { get; set; }
        public string Notes { get; set; }
        public string RefNbr { get; set; }
        public string Description { get; set; }
        public string Payment { get; set; }
        public string VoucherType { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Boolean? isDelete { get; set; } //hvtam-19022016 add flag isDeleted

        public string PaymentMethod { get { return Payment == "C" ? "Cash" : "TT"; } }
        public string VoucherTypeString { get { return VoucherType == "R" ? "Receipt" : "Payment"; } }
        public decimal? ReceiptAmount { get { return VoucherType == "R" ? Amount : 0; } }
        public decimal? PaymentAmount { get { return VoucherType == "P" ? Amount : 0; } }
    }
}
