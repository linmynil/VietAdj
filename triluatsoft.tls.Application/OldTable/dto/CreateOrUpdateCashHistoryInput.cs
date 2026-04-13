using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreateOrUpdateCashHistoryInput
    {
        public int? Id { get; set; }
        public string CashCode { get; set; }
        public string RefNbr { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string PaymentMethod { get; set; }
        public string VoucherType { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> DateTimeNow { get; set; }
    }
}
