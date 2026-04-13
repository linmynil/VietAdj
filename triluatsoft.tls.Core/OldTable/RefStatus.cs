using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("tblRefStatus")]
    public class RefStatus: Entity
    {
        public string Name { get; set; }
    }
    public class RefStatusViewColumn
    {
        public const string REFSTATUS_ID = "ID";
        public const string REFSTATUS_NAME = "Name";
    }
    public class REF_STATUS_DEFINE
    {
        public const int OPEN = 1;
        public const int CLOSE = 2;
        public const int REOPEN = 3;
    }
    public class REF_STATUSNAME_DEFINE
    {
        public const string REF_NAME_OPEN = "Outstanding";
        public const string REF_NAME_CLOSED = "Closed";
        public const string REF_NAME_REOPEN = "Re-Opened";
    }
}
