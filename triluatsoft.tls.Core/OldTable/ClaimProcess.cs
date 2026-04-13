using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("ClaimProcess")]
    public class ClaimProcess : Entity
    {
        public string ClaimID { get; set; }
        public Nullable<bool> IsAck { get; set; }
        public Nullable<System.DateTime> AckDeadline { get; set; }
        public Nullable<System.DateTime> AckTime { get; set; }
        public Nullable<int> AckBy { get; set; }
        public Nullable<bool> IsDocRequest { get; set; }
        public Nullable<System.DateTime> DocRequestDeadline { get; set; }
        public Nullable<System.DateTime> DocRequestTime { get; set; }
        public Nullable<int> DocRequestBy { get; set; }
        public Nullable<bool> IsMeetingNote { get; set; }
        public Nullable<System.DateTime> MeetingNoteDeadline { get; set; }
        public Nullable<System.DateTime> MeetingNoteTime { get; set; }
        public Nullable<int> MeetingNoteBy { get; set; }
        public Nullable<bool> IsILA { get; set; }
        public Nullable<System.DateTime> ILADeadline { get; set; }
        public Nullable<System.DateTime> ILATime { get; set; }
        public Nullable<int> ILABy { get; set; }
        public Nullable<bool> IsPREL1 { get; set; }
        public Nullable<System.DateTime> PREL1Deadline { get; set; }
        public Nullable<System.DateTime> PREL1Time { get; set; }
        public Nullable<int> PREL1By { get; set; }
        public Nullable<bool> IsPREL2 { get; set; }
        public Nullable<System.DateTime> PREL2Deadline { get; set; }
        public Nullable<System.DateTime> PREL2Time { get; set; }
        public Nullable<int> PREL2By { get; set; }
        public Nullable<bool> IsPREHC { get; set; }
        public Nullable<System.DateTime> PREHCDeadline { get; set; }
        public Nullable<System.DateTime> PREHCTime { get; set; }
        public Nullable<int> PREHCBy { get; set; }
        public Nullable<bool> IsINTL1 { get; set; }
        public Nullable<System.DateTime> INTL1Deadline { get; set; }
        public Nullable<System.DateTime> INTL1Time { get; set; }
        public Nullable<int> INTL1By { get; set; }
        public Nullable<bool> IsINTL2 { get; set; }
        public Nullable<System.DateTime> INTL2Deadline { get; set; }
        public Nullable<System.DateTime> INTL2Time { get; set; }
        public Nullable<int> INTL2By { get; set; }
        public Nullable<bool> IsINTHC { get; set; }
        public Nullable<System.DateTime> INTHCDeadline { get; set; }
        public Nullable<System.DateTime> INTHCTime { get; set; }
        public Nullable<int> INTHCBy { get; set; }
        public Nullable<bool> IsFINL1 { get; set; }
        public Nullable<System.DateTime> FINL1Deadline { get; set; }
        public Nullable<System.DateTime> FINL1Time { get; set; }
        public Nullable<int> FINL1By { get; set; }
        public Nullable<bool> IsFINL2 { get; set; }
        public Nullable<System.DateTime> FINL2Deadline { get; set; }
        public Nullable<System.DateTime> FINL2Time { get; set; }
        public Nullable<int> FINL2By { get; set; }
        public Nullable<bool> IsFINHC { get; set; }
        public Nullable<System.DateTime> FINHCDeadline { get; set; }
        public Nullable<System.DateTime> FINHCTime { get; set; }
        public Nullable<int> FINHCBy { get; set; }
        public Nullable<bool> IsSubmit { get; set; }
        public Nullable<System.DateTime> SubmitDeadline { get; set; }
        public Nullable<System.DateTime> SubmitTime { get; set; }
        public Nullable<int> SubmitBy { get; set; }
        public Nullable<bool> IsFirstSurvey { get; set; }        
        public Nullable<System.DateTime> FirstSurveyTime { get; set; }
        public Nullable<int> FirstSurveyBy { get; set; }
        public Nullable<bool> IsCompleteSurvey { get; set; }
        public Nullable<System.DateTime> CompleteSurveyTime { get; set; }
        public Nullable<int> CompleteSurveyBy { get; set; }
        public Nullable<bool> IsRequestPREL1 { get; set; }
        public Nullable<System.DateTime> RequestPREL1Time { get; set; }
        public Nullable<int> RequestPREL1By { get; set; }
        public Nullable<bool> IsRequestPREL2 { get; set; }
        public Nullable<System.DateTime> RequestPREL2Time { get; set; }
        public Nullable<int> RequestPREL2By { get; set; }
        public Nullable<bool> IsConfirmPRE { get; set; }
        public Nullable<System.DateTime> ConfirmPRETime { get; set; }
        public Nullable<int> ConfirmPREBy { get; set; }
        public Nullable<bool> IsRequestINTL1 { get; set; }
        public Nullable<System.DateTime> RequestINTL1Time { get; set; }
        public Nullable<int> RequestINTL1By { get; set; }
        public Nullable<bool> IsRequestINTL2 { get; set; }
        public Nullable<System.DateTime> RequestINTL2Time { get; set; }
        public Nullable<int> RequestINTL2By { get; set; }
        public Nullable<bool> IsConfirmINT { get; set; }
        public Nullable<System.DateTime> ConfirmINTTime { get; set; }
        public Nullable<int> ConfirmINTBy { get; set; }
        public Nullable<bool> IsRequestFINL1 { get; set; }
        public Nullable<System.DateTime> RequestFINL1Time { get; set; }
        public Nullable<int> RequestFINL1By { get; set; }
        public Nullable<bool> IsRequestFINL2 { get; set; }
        public Nullable<System.DateTime> RequestFINL2Time { get; set; }
        public Nullable<int> RequestFINL2By { get; set; }
        public Nullable<bool> IsConfirmFIN { get; set; }
        public Nullable<System.DateTime> ConfirmFINTime { get; set; }
        public Nullable<int> ConfirmFINBy { get; set; }
    }
}
