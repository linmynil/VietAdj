using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    //[UserCredential]
    public class UserView
    {
        public string ID { get; set; }
        public int EmployeeID { get; set; }
        public int? SourceID { get; set; } //hvtam-072014: SourceID ->EmployeeID
        public bool IsActive { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }

        public List<SecGroupView> AssignedList { get; set; }
    }

    public class UserViewColumn
    {
        public const string USER_ID = "ID";
        public const string USER_NAME = "Name";
    }
}
