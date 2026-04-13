using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreatePaymentInput
    {
        public int LiabilitiesID { get; set; }
        public string PaymentCode { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaymentAMT { get; set; }
        public string RefCode { get; set; }
        public string Remark { get; set; }
    }
}
