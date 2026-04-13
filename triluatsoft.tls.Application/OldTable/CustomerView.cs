using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    public class CustomerView
    {
        public CustomerView()
        {
            Accounts = new List<UserView>();
        }

        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
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

        public bool HasAccount { get { return Accounts.Count > 0; } }
        public List<UserView> Accounts { get; set; }

        public override int GetHashCode()
        {
            return CustomerID.GetHashCode() ^ (CustomerName == null ? 0 : CustomerName.GetHashCode());
        }
    }
}
