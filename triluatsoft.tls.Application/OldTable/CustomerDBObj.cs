using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    /// <summary>
    /// TODO replace by Customer entity
    /// </summary>
    public class CustomerDBObj
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactPosition { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public bool? IsActive { get; set; }
        public string RecordType { get; set; }
        public decimal? Balance { get; set; }
        public int? TranID { get; set; }
        public decimal? BalanceDebit { get; set; }
        public decimal? BalanceCredit { get; set; }
    }
}
