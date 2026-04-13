using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class ClaimFolderInfo
    {
        public int ID { get; set; }
        public string ClaimID { get; set; }
        public string FName { get; set; }
        public string FPath { get; set; }
        public int FSecurityID { get; set; }
        public string FSecurity { get; set; }
        public string catalog { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
