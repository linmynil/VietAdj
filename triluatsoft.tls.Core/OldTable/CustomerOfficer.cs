using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("CustomerOfficer")]
    public class CustomerOfficer : Entity
    {
        public CustomerOfficer()
        {
            this.Commissions = new HashSet<Commission>();
        }

        public int OfficerID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<Commission> Commissions { get; set; }
    }
}
