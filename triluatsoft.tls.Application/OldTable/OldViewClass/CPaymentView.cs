using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.OldViewClass
{
    /// <summary>
    /// copy from old project
    /// </summary>
    public class CPaymentView
    {

        public int TransactionID { get; set; }
        public int CustomerID { get; set; }
        public int InvoiceID { get; set; }

        public int? LiabilitiesID { get; set; }

        public string PaymentCode { get; set; }
        public string RefCode { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? PaymentAMT { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Remark { get; set; }
        public decimal? CurrentBalanceDebit { get; set; }
        public decimal? CurrentBalanceCredit { get; set; }
        public int TranID { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool IsDelete { get; set; }

        //-----------------------------------
        public string ClaimID { get; set; }     //Bo
        public string InvoiceCode { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CreateByName { get; set; }
        public string CustomerName { get; set; }
        public bool isNonInvoice { get; set; }//hvtam-16122014

    }
    //hvtam-08122014
    public class CMSPaymentView
    {

        public int TransactionID { get; set; }
        public int CommissionID { get; set; }
        public string PaymentCode { get; set; }
        public string RefCode { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? PaymentAMT { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string OfficerFullname { get; set; }
        public string InvoiceCode { get; set; }


    }
    //hvtam-08122014
    public class PAYMENT_TYPE_DEFINE
    {

        public const string PAYMENT_TYPE_PAYMENT = "P"; //Payment
        public const string PAYMENT_TYPE_RECEIVE = "R"; //Receivment
        public const string PAYMENT_TYPE_MAP = "M"; //MAP ADVINVOICE FOR TIMESHEET
    }
    //hvtam-08122014
    public class PAYMENT_METHOD_DEFINE
    {

        public const string PAYMENT_METHOD_CASH = "C";
        public const string PAYMENT_METHOD_BANK = "B";
        public const string PAYMENT_METHOD_MAP = "M";
    }
}
