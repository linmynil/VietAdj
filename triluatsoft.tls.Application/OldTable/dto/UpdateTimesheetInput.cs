using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class UpdateTimesheetInput
    {
        public int TimeSheetID { get; set; }
        public decimal? DiscountVal { get; set; }
        public string DiscountType { get; set; }
        public List<UpdateProFeeInput> ListProFee { get; set; }
        public decimal? ExchangeRate { get; set; }
    }

    public class UpdateProFeeInput
    {
        public int ProfessionalFeeID { get; set; }

        public string WorkTime { get; set; }
        public string ApproveTime { get; set; }

        public decimal? ChargedFeePerHour { get; set; }
        public int? ChargedBy { get; set; }        
    }
}
