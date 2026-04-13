using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreateOrUpdateFeeInput
    {
        /// <summary>
        /// mapping to InputDate
        /// </summary>
        public DateTime? InputDate { get; set; }
        public int JobCodeID { get; set; }

        public string Notes { get; set; }
        public int? ProfessionalFeeID { get; set; }

        public decimal StandardTime { get; set; }

        /// <summary>
        /// actual time: hh:mm
        /// </summary>
        public string WorkTime { get; set; }
        public int TimeSheetID { get; set; }
    }
}
