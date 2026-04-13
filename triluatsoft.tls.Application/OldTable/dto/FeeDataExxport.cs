using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Datasets;

namespace triluatsoft.tls.OldTable.dto
{
    public class FeeDataExport
    {
        public ProfeeDS dataset { get; set; }
        public string Refer { get; set; }
        public string Adjuster { get; set; }
        public decimal exchangeRate { get; set; }
        public decimal userFee { get; set; }
        public decimal userFeeVND { get; set; }
        public string TotalWorkTime { get; set; }
        public decimal TotalAmount { get; set; }        
    }
}
