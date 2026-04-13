using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("Customer")]
    public class Customer : Entity<int>
    {       
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
        public Nullable<bool> IsActive { get; set; }
        public string RecordType { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<int> TranID { get; set; }
        public Nullable<decimal> BalanceDebit { get; set; }
        public Nullable<decimal> BalanceCredit { get; set; }
    }
}
