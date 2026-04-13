using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    //[Employee]
    public class EmployeeView : IEquatable<EmployeeView>
    {
        public EmployeeView()
        {
            Accounts = new List<UserView>();
        }
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string JobPosition { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal? Fee { get; set; }
        public bool IsActive { get; set; }
        public bool? IsAuthen { get; set; } //hvtam-07112014

        public bool HasAccount { get { return Accounts.Count > 0; } }
        public List<UserView> Accounts { get; set; }

        public bool Equals(EmployeeView other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return this.EmployeeID == other.EmployeeID;
        }

        public override int GetHashCode()
        {
            return EmployeeID.GetHashCode() ^ (Name == null ? 0 : Name.GetHashCode());
        }
    }
}
