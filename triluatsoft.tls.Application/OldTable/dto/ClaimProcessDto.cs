using System;

namespace triluatsoft.tls.OldTable.dto
{    
    public class ClaimProcessDto
    {
        public int Id { get; set; }
        public string ClaimID { get; set; }
        public string LastStatus { get; set; }
        public int NextAction { get; set; }
        public bool WaitDocRequest { get; set; }        
        public bool EnPREL2Request { get; set; }
        public bool EnINTL2Request { get; set; }
        public bool EnFINL2Request { get; set; }
    }
}
