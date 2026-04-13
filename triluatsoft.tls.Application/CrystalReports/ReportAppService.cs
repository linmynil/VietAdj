using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using triluatsoft.tls.Datasets;
using triluatsoft.tls.DataExporting;
using triluatsoft.tls.Dto;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.Net.MimeTypes;
using triluatsoft.tls.OldTable;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.OldViewClass;
using triluatsoft.tls.Authorization;

namespace triluatsoft.tls.CrystalReports
{
    public class ReportAppService : tlsAppServiceBase, IReportAppService
    {
        public IAppFolders AppFolders { get; set; }
        private readonly IClaimAppService _claimAppService;
        private readonly IRepository<TimeSheet> _timeSheetRepo;
        private readonly IRepository<Invoice> _invoiceRepo;
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IBorderauxStatusAppService _bordereauxStatusService;
        private readonly IEmployeeAppService _empService;
        private readonly ITimeSheetAppService _timesheetService;
        public ReportAppService(IClaimAppService claimAppService, IRepository<TimeSheet> timeSheetRepo
            , IRepository<Invoice> invoiceRepo
            , ISqlExecuter sqlExecuter
            , IBorderauxStatusAppService bordereauxStatusService
            , IEmployeeAppService empService
            , ITimeSheetAppService timesheetService)
        {
            _claimAppService = claimAppService;
            _timeSheetRepo = timeSheetRepo;
            _invoiceRepo = invoiceRepo;
            _sqlExecuter = sqlExecuter;
            _bordereauxStatusService = bordereauxStatusService;
            _empService = empService;
            _timesheetService = timesheetService;
        }
        public DataTable ReportTimeSheetSum(string ClaimID, ref dynamic ClaimSummary)
        {

            ClaimSummary = new ExpandoObject();
            DataTable dataReport = new DataTable("Invoice");
            dataReport.Columns.Add("InvoiceID", typeof(int));
            dataReport.Columns.Add("InvoiceCode", typeof(string));
            dataReport.Columns.Add("InvoiceDate", typeof(DateTime));
            dataReport.Columns.Add("InvoiceAmount", typeof(decimal));

            var claimInfo = _claimAppService.GetById(ClaimID);
            ClaimSummary.TheInsured = claimInfo.TheInsured ?? string.Empty;


            var invlst = (from i in _invoiceRepo.GetAll()
                          where i.ClaimID == ClaimID
                          orderby i.InvoiceDate ascending
                          select new
                          {
                              InvoiceID = i.Id,
                              InvoiceCode = i.InvoiceCode,
                              InvoiceDate = i.InvoiceDate,
                              TotalAMT = i.TotalAMT,
                              TaxAMT = i.TaxAMT,
                          }).ToList();

            var TotalInvoiceAMT = (dynamic)null;
            if (invlst.Count != 0) //hvtam-18112014: Draw Invoice lines
            {
                foreach (var i in invlst.ToList())
                {
                    DataRow dr = dataReport.NewRow();
                    dr["InvoiceID"] = i.InvoiceID;
                    dr["InvoiceCode"] = i.InvoiceCode ?? string.Empty;
                    dr["InvoiceDate"] = i.InvoiceDate ?? DateTime.MinValue;
                    dr["InvoiceAmount"] = i.TotalAMT ?? 0;
                    dataReport.Rows.Add(dr);
                }
                TotalInvoiceAMT = _invoiceRepo.GetAll().Where(i => i.ClaimID == ClaimID).Sum(i => i.TotalAMT).Value; //hvtam-18112014
            }
            else
            {
                TotalInvoiceAMT = 0;
            }
            ClaimSummary.TotalInvoiceAMT = TotalInvoiceAMT;

            var tsSum = (from t in _timeSheetRepo.GetAll()
                         where t.ClaimID == ClaimID
                         group t by new { t.ClaimID } into g
                         select new
                         {
                             ClaimID = g.Key.ClaimID,
                             ExchangeRate = g.Average(t => t.ExchangeRate),
                             ProFeeAMT = g.Sum(t => t.ProFeeAMT),
                             //ProFeeGrandAMT = g.Sum(t =>t.IsIssued == true? t.ProFeeGrandAMT: t.ActualProFeeGrandAMT), hvtam-25042015: Neu tinh WIP theo actualtime nua thi phai them colum ActualProFeeGrandAM vao timesheet table, gia tri nay luu khi nhap actual profee 
                             ProFeeGrandAMT = g.Sum(t => t.ProFeeGrandAMT),
                             ExpenseAMT = g.Sum(t => t.ExpenseAMT),
                             TaxAMT = g.Sum(t => t.TaxAMT),
                             GrandAMT = g.Sum(t => t.GrandAMT),
                         }).FirstOrDefault();

            if (tsSum != null)
            {
                ClaimSummary.ExchangeRate = tsSum.ExchangeRate;
                ClaimSummary.ProFeeAMT = tsSum.ProFeeAMT;
                ClaimSummary.ProFeeGrandAMT = tsSum.ProFeeGrandAMT;
                ClaimSummary.ExpenseAMT = tsSum.ExpenseAMT;
                ClaimSummary.TotalAMT = tsSum.ExpenseAMT + tsSum.ProFeeGrandAMT;
                if (tsSum.TaxAMT != null)
                    ClaimSummary.TaxAMT = tsSum.TaxAMT;
                else
                    ClaimSummary.TaxAMT = 0;
                ClaimSummary.GrandAMT = tsSum.GrandAMT;
                ClaimSummary.TotalInvoiceAMT = TotalInvoiceAMT;
                ClaimSummary.RemainAMT = (tsSum.GrandAMT - TotalInvoiceAMT);
            }
            else
            {
                ClaimSummary.ExchangeRate = 0;
                ClaimSummary.ProFeeAMT = 0;
                ClaimSummary.ProFeeGrandAMT = 0;
                ClaimSummary.ExpenseAMT = 0;
                ClaimSummary.TotalAMT = 0;
                ClaimSummary.TaxAMT = 0;
                ClaimSummary.GrandAMT = 0;
                ClaimSummary.RemainAMT = (0 - TotalInvoiceAMT);
            }

            ClaimSummary.ClaimID = ClaimID;
            ClaimSummary.RiskLocation = "claimInfo.RiskLocation" ?? string.Empty;
            ClaimSummary.DateOfLoss = new DateTime(1990, 1, 1);
            Logger.Debug("Report_TimeSheetSum");
            return dataReport;
        }
        
        /// <summary>
        ///     Get Bordereaux Report
        ///     Doi ten ham GetClaimsByInsurerID thanh GetBordereauxReport
        /// </summary>
        /// <param name="insurerID"></param>
        /// <returns></returns>
        public DataTable GetBordereauxReportNEW(int insurerID)
        {
            //string customerName;
            //string contactName;


            var context = _sqlExecuter.GetTLSDBContext();
            {
                //var customer = context.Customers.Where(c => c.ID == insurerID)
                //                                .Select(c => new
                //                                {
                //                                    CustomerName =
                //                                        c.Name +
                //                                        (c.BrandName == null || c.BrandName == ""
                //                                            ? string.Empty : " (" + c.BrandName + ")"),
                //                                    ContactName =
                //                                        (c.ContactTitle == null || c.ContactTitle == ""
                //                                            ? string.Empty : c.ContactTitle + " ") +
                //                                        c.ContactName +
                //                                        (c.ContactPosition == null || c.ContactPosition == ""
                //                                            ? string.Empty : " - " + c.ContactPosition)
                //                                })
                //                                .SingleOrDefault();

                //customerName = customer.CustomerName;

                //contactName = customer.ContactName ?? string.Empty;

                var query = (from c in context.Claims
                                 //hvtam-29022016
                                 //join h in context.Bordereaux on c.ID equals h.ClaimID
                             join h in context.Bordereauxs on c.BordereauxID equals h.Id
                             //join bStatus in context.ClaimStatus on h.ClaimStatusID equals bStatus.ID
                             //endhvtam-29022016
                             //join cz in context.Causes on c.CauseID equals cz.ID
                             where c.InsurerID == insurerID
                                     && c.RefStatusID != REF_STATUS_DEFINE.CLOSE //hvtam-04122015: chi hien thi nhung claim chua close
                             //group h.ClaimStatusID by c into g
                             orderby c.DateOfAssignment ascending    //hvtam-07032016
                             select new
                             {
                                 //ClaimID = g.Key.ID,
                                 //Insured = g.Key.TheInsured,
                                 //PolicyNo = g.Key.PolicyNo,
                                 //CauseOfLoss = g.Key.OtherCause != null || g.Key.OtherCause != ""
                                 //    ? g.Key.OtherCause : g.Key.Cause != null ? g.Key.Cause.Name : string.Empty,
                                 //ClientsRef = g.Key.ClientsRef,
                                 //Report = g.Key.LastHistory.Report.Name,
                                 //Reserve = g.Key.Reserve,
                                 //StatusList = g.Select(e => e).Distinct()
                                 ClaimID = c.Id,
                                 Insured = c.TheInsured,
                                 PolicyNo = c.PolicyNo,
                                 CauseOfLoss = c.OtherCause != null && c.OtherCause != string.Empty ? c.OtherCause
                                                                      : c.CauseID != null ? c.Cause.Name : string.Empty,
                                 ClientsRef = c.ClientsRef,
                                 Report = h.Report.Name,
                                 StatusRemarks = h.BordereauxStatusID,
                                 Reserve = h.Reserve,
                                 ProgressPayment = h.ProgressPayment,
                                 //BalanceOfReserve = h.Reserve - h.ProgressPayment,
                                 DateofAssignment = c.DateOfAssignment
                                 //StatusList = g.Select(e => e).Distinct()

                             }).Concat
                            (
                                from c in context.Claims
                                from co in context.CoOwnerClaims
                                    //hvtam-29022016
                                    //join h in context.Bordereaux on c.ID equals h.ClaimID
                                join h in context.Bordereauxs on c.BordereauxID equals h.Id
                                //join bStatus in context.ClaimStatus on h.ClaimStatusID equals bStatus.ID
                                //endhvtam-29022016
                                //join cz in context.Causes on c.CauseID equals cz.ID
                                where c.Id == co.ClaimID && co.CustomerID == insurerID
                                        && c.RefStatusID != REF_STATUS_DEFINE.CLOSE //hvtam-04122015: chi hien thi nhung claim chua close
                                //group h.ClaimStatusID by c into g
                                orderby c.DateOfAssignment ascending    //hvtam-07032016
                                select new
                                {
                                    //ClaimID = g.Key.ID,
                                    //Insured = g.Key.TheInsured,
                                    //PolicyNo = g.Key.PolicyNo,
                                    //CauseOfLoss = g.Key.OtherCause != null || g.Key.OtherCause != ""
                                    //    ? g.Key.OtherCause : g.Key.Cause != null ? g.Key.Cause.Name : string.Empty,
                                    //ClientsRef = g.Key.ClientsRef,
                                    //Report = g.Key.LastHistory.Report.Name,
                                    //Reserve = g.Key.Reserve,
                                    //StatusList = g.Select(e => e).Distinct()
                                    ClaimID = c.Id,
                                    Insured = c.TheInsured,
                                    PolicyNo = c.PolicyNo,
                                    CauseOfLoss = c.OtherCause != null && c.OtherCause != string.Empty ? c.OtherCause
                                                                         : c.CauseID != null ? c.Cause.Name : string.Empty,
                                    ClientsRef = c.ClientsRef,
                                    Report = h.Report.Name,
                                    StatusRemarks = h.BordereauxStatusID,
                                    Reserve = h.Reserve,
                                    ProgressPayment = h.ProgressPayment,
                                    //BalanceOfReserve = h.Reserve - h.ProgressPayment,
                                    DateofAssignment = c.DateOfAssignment
                                    //StatusList = g.Select(e => e).Distinct()
                                }
                            ).OrderByDescending(x => x.DateofAssignment);

                //foreach (var c in query.ToList())
                //{
                //    DataRow dr = dt.NewRow();
                //    dr["ClaimID"] = c.ClaimID;
                //    dr["Insured"] = c.Insured ?? string.Empty;
                //    dr["PolicyNo"] = c.PolicyNo ?? string.Empty;
                //    dr["CauseOfLoss"] = string.IsNullOrWhiteSpace(c.CauseOfLoss) ? "Under Investigation" : c.CauseOfLoss;
                //    dr["ClientsRef"] = string.IsNullOrWhiteSpace(c.ClientsRef) ? "To be Advised" : c.ClientsRef;    //Please Advice => TO BE ADVISED
                //    dr["Report"] = c.Report ?? string.Empty;
                //    dr["Reserve"] = c.Reserve.HasValue ? c.Reserve.ToString() : "Under Investigation"; //hvtam-20022016
                //    dr["ProgressPayment"] = c.ProgressPayment.HasValue ? c.ProgressPayment.ToString() : "Under Investigation";  //hvtam-20022016
                //    dr["BalanceOfReserve"] = c.BalanceOfReserve.HasValue ? c.BalanceOfReserve.ToString() : "-";
                //    //dr["StatusRemarks"] = string.Join(", ", c.StatusList);
                //    dr["StatusRemarks"] = c.StatusID.ToString() ?? string.Empty;
                //    dr["DateofAssignment"] = c.DateofAssignment ?? DateTime.MinValue;   //hvtam-tam vay
                //    dt.Rows.Add(dr);
                //}
                //stored result into datatable  
                DataTable dt = new DataTable("ClaimTBL");
                dt = LINQResultToDataTable(query);
                //dynamic data = new ExpandoObject();
                //data.CustomerName = customerName ?? string.Empty;
                //data.ContactName = contactName ?? string.Empty;
                //data.Table = dt;
                //return data;
                return dt;
            }


        }


        public dynamic GetChargedProfeeOfTSByUser(int TimesheetID, int empID, decimal exchRate)
        {
            //string userName;
            decimal userFee;
            //decimal exchRate;         //hvtam-25122014
            decimal TotalAmount = 0;    //hvtam-21112014
            string TotalWorkTime;       //hvtam-21112014
            int TotalMinutes = 0;       //hvtam-21112014
            //string claimID;             //hvtam-21112014 : hvtam-15122014: Ko can nua, su dung timesheetname roi

            DataTable dt = new DataTable("TimeSheetTBL"); //Define TimeSheetTBL Table
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Description", typeof(string));
            //dt.Columns.Add("WorkingHour", typeof(TimeSpan));
            dt.Columns.Add("strWorkingHour", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));


            var context = _sqlExecuter.GetTLSDBContext();
            {

                var query = from t in context.TimeSheets
                            join pf in context.ProfessionalFees on t.Id equals pf.TimeSheetID
                            join emp in context.Users on pf.ChargedBy equals emp.EmployeeId
                            where (t.Id == TimesheetID && pf.ChargedBy == empID)
                            select new
                            {
                                Date = pf.InputDate,
                                Description = (pf.Notes != null ? pf.Notes : string.Empty),
                                ApproveTime = pf.ApproveTime != null ? pf.ApproveTime : pf.WorkTime,
                                //ApproveTime = t.ApproveTime != null ? t.ApproveTime : TimeSpan.Zero,
                                //hvtam-02122015 UserFee = (pf.ChargedFeePerHour ==null? pf.FeePerHour:pf.ChargedFeePerHour),//UserFee = t.FeePerHour,
                                UserFee = emp.Fee,
                                //exchRate = t.ExchangeRate??0,
                                ProFeeAMTUSD = (pf.ChargedProFeeValue != null ? pf.ChargedProFeeValue : pf.ProFeeValue ?? 0), //hvtam-22042015   
                            };
                var pfList = query.ToList();
                foreach (var i in pfList)
                {

                    TimeSpan approvetime = (TimeSpan)(i.ApproveTime ?? TimeSpan.Zero);
                    int minute = approvetime.Hours * 60 + approvetime.Minutes;
                    TotalMinutes = TotalMinutes + minute;
                    ////hvtam-20122014 dr["Amount"] = i.Amount ?? 0; db chi luu nhung truong hop da issue roi, nen phai tinh
                    ////decimal h = minute / 60;
                    ////int m = workingtime.Minutes;
                    //decimal decimalHour = Convert.ToDecimal(approvetime.TotalHours); //hvtam-13032015: Ham de convert tu h:m thanh decimal hour
                    ////decimal amount = (minute / 60 + minute % 60) * (i.UserFee ?? 0) * exchRate;
                    //decimal amount = decimalHour * (i.UserFee ?? 0) * exchRate;
                    //decimal profeevalue = TimeSheetServiceUOW.ProfeeValueCalVND(i.ApproveTime, i.UserFee, exchRate);
                    //TotalAmount = TotalAmount + profeevalue;

                    DataRow dr = dt.NewRow();
                    dr["Date"] = i.Date ?? DateTime.MinValue;
                    dr["Description"] = i.Description ?? string.Empty;
                    //dr["WorkingHour"] = (TimeSpan)(i.WorkingHour?? TimeSpan.Zero);  //hvtam-20112014 check again???                                                                     
                    dr["strWorkingHour"] = approvetime.Hours + ":" + approvetime.Minutes;
                    //dr["Amount"] = profeevalue;                    
                    dt.Rows.Add(dr);
                }

                userFee = pfList.Max(ts => ts.UserFee) ?? 0;
                TotalWorkTime = TotalMinutes / 60 + ":" + TotalMinutes % 60;
                //TotalWorkTime = new TimeSpan(Sum(i.WorkingHour.Ticks));
                TotalAmount = pfList.Sum(pf => pf.ProFeeAMTUSD ?? 0) * exchRate;
            }


            //hvtam-21112014 comment Expand new data of table
            dynamic data = new ExpandoObject();
            //data.UserName = userName;
            data.UserFee = userFee;
            //data.ExchangeRate = exchRate;     //hvtam-25122014: Lay tu timesheetview roi.
            data.TotalWorkTime = TotalWorkTime; //hvtam-21112014
            data.TotalAmount = TotalAmount;     //hvtam-21112014
            //data.claimID = claimID;
            data.Table = dt;
            return data;
        }



        //***********************************************************************************************************
        //hvtam-20112014: Ham cua hvtam
        //hvtam-02122015: Doi ten ham GetProfeeByUser -> GetChargedProfeeOfTSByUser
        //GetTimeSheetByUser(string ClaimID, int empID) -> GetProfeeByUser(int TimesheetID, int empID) ->GetChargedProfeeOfTSByUser
        //************************************************************************************************************
        private dynamic GetUserProfee4ExportExcel(int TimesheetID, int empID, decimal exchRate, string lang)
        {
            //string userName;
            decimal userFee;
            //decimal exchRate;         //hvtam-25122014
            decimal TotalAmount = 0;    //hvtam-21112014
            string TotalWorkTime;       //hvtam-21112014
            int TotalMinutes = 0;       //hvtam-21112014
            //string claimID;             //hvtam-21112014 : hvtam-15122014: Ko can nua, su dung timesheetname roi
            string Adjuster = "";

            DataTable dt = new DataTable(); //Define TimeSheetTBL Table
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Col1", typeof(string));
            dt.Columns.Add("Description", typeof(string));                        
            dt.Columns.Add("Time (hh:mm)", typeof(string));            
            dt.Columns.Add("Amount (VND)", typeof(decimal));
            
            var context = _sqlExecuter.GetTLSDBContext();

            var query = from t in context.TimeSheets
                        join pf in context.ProfessionalFees on t.Id equals pf.TimeSheetID
                        join emp in context.Users on pf.ChargedBy equals emp.EmployeeId
                        where (t.Id == TimesheetID && pf.ChargedBy == empID)
                        select new
                        {
                            Date = pf.InputDate,
                            Description = (pf.Notes != null ? pf.Notes : string.Empty),
                            ApproveTime = pf.ApproveTime != null ? pf.ApproveTime : pf.WorkTime,
                            Adjuster = emp.Name + " - " + emp.JobPosition,
                            //ApproveTime = t.ApproveTime != null ? t.ApproveTime : TimeSpan.Zero,
                            //hvtam-02122015 UserFee = (pf.ChargedFeePerHour ==null? pf.FeePerHour:pf.ChargedFeePerHour),//UserFee = t.FeePerHour,
                            UserFee = emp.Fee,
                            //exchRate = t.ExchangeRate??0,
                            ProFeeAMTUSD = (pf.ChargedProFeeValue != null ? pf.ChargedProFeeValue : pf.ProFeeValue ?? 0), //hvtam-22042015   
                        };
            var pfList = query.ToList();
            foreach (var i in pfList)
            {

                TimeSpan approvetime = (TimeSpan)(i.ApproveTime ?? TimeSpan.Zero);
                int minute = approvetime.Hours * 60 + approvetime.Minutes;
                TotalMinutes = TotalMinutes + minute;
                ////hvtam-20122014 dr["Amount"] = i.Amount ?? 0; db chi luu nhung truong hop da issue roi, nen phai tinh
                ////decimal h = minute / 60;
                ////int m = workingtime.Minutes;
                //decimal decimalHour = Convert.ToDecimal(approvetime.TotalHours); //hvtam-13032015: Ham de convert tu h:m thanh decimal hour
                ////decimal amount = (minute / 60 + minute % 60) * (i.UserFee ?? 0) * exchRate;
                //decimal amount = decimalHour * (i.UserFee ?? 0) * exchRate;
                //decimal profeevalue = TimeSheetServiceUOW.ProfeeValueCalVND(i.ApproveTime, i.UserFee, exchRate);
                //TotalAmount = TotalAmount + profeevalue;
                Adjuster = i.Adjuster;

                DataRow dr = dt.NewRow();
                dr["Date"] = i.Date ?? DateTime.MinValue;
                dr["Col1"] = string.Empty;
                dr["Description"] = i.Description ?? string.Empty;                                
                dr["Time (hh:mm)"] = "'" + approvetime.Hours.ToString("D2") + ":" + approvetime.Minutes.ToString("D2");                
                dr["Amount (VND)"] = i.ProFeeAMTUSD * exchRate;
                dt.Rows.Add(dr);
            }            

            userFee = pfList.Max(ts => ts.UserFee) ?? 0;
            TotalWorkTime = (TotalMinutes / 60).ToString("D2") + ":" + (TotalMinutes % 60).ToString("D2");
            //TotalWorkTime = new TimeSpan(Sum(i.WorkingHour.Ticks));
            TotalAmount = pfList.Sum(pf => pf.ProFeeAMTUSD ?? 0) * exchRate;            

            //hvtam-21112014 comment Expand new data of table
            dynamic data = new ExpandoObject();
            //data.UserName = userName;
            data.Adjuster = Adjuster;
            data.UserFee = userFee;
            //data.ExchangeRate = exchRate;     //hvtam-25122014: Lay tu timesheetview roi.
            data.TotalWorkTime = TotalWorkTime; //hvtam-21112014
            data.TotalAmount = TotalAmount;     //hvtam-21112014
            //data.claimID = claimID;
            data.Table = dt;
            return data;
        }


        public FileDto ExportTimesheetFeeReport(int tsID, string lang)
        {

            //OldTable.EmployeeView emp = _empService.GetEmpInfo(empID);
            //string reportEmpName = emp.Name + " - " + emp.JobTitle + "," + emp.JobPosition;
            OldTable.OldViewClass.TimeSheetView TSView = _timesheetService.GetTSInfo(tsID);

            if (TSView == null)
                return null;

            //List<DataSet> dataSets = new List<DataSet>();
            List<FeeDataExport> exportData = new List<FeeDataExport>();

            List<EmployeeView> lstEmp = _empService.GetEmployeeListByClaim(TSView.ClaimID);
            foreach (EmployeeView empl in lstEmp)

            {
                FeeDataExport dtexp = new FeeDataExport();
                dtexp.dataset = new ProfeeDS();
                //ProfeeDS ds = new ProfeeDS();
                dynamic data1 = GetUserProfee4ExportExcel(tsID, empl.EmployeeID, TSView.ExchangeRate ?? 1, lang);

                //ds.Tables[0].Merge(data1.Table);
                //ds.DataSetName = empl.Name;

                //dataSets.Add(ds);

                dtexp.dataset.Tables[0].Merge(data1.Table);
                dtexp.dataset.DataSetName = empl.Name;
                dtexp.Refer = TSView.TimeSheetName;
                dtexp.Adjuster = data1.Adjuster;
                dtexp.userFee = data1.UserFee;
                dtexp.exchangeRate = TSView.ExchangeRate ?? 1;
                dtexp.TotalWorkTime = "'" + data1.TotalWorkTime;
                dtexp.userFeeVND = dtexp.exchangeRate * dtexp.userFee;
                dtexp.TotalAmount = data1.TotalAmount;

                exportData.Add(dtexp);
            }

            string path = AppFolders.TempFileDownloadFolder;
            //System.Configuration.ConfigurationManager.AppSettings["ExportFolder"];
            string filename = TSView.TimeSheetName + "_" + lang + ".xlsx";

            var file = new FileDto(filename, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, filename);

            //ExcelService.DataSetsToExcel(dataSets, path, filename);
            ExcelService.DataSetsToExcel(exportData, path, filename, lang);

            return file;
        }

        public DataTable GetBordereauxStatusList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Name", typeof(string));

            foreach (var s in _bordereauxStatusService.GetActiveList())//hvtam-19032016 foreach (var s in ClaimServiceUOW.GetStatusList())
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = s.StatusID;
                dr["Name"] = s.Name ?? string.Empty;
                dt.Rows.Add(dr);
            }

            return dt;
        }
        public dynamic GetExpensesListByTimeSheetID(int TimesheetID)
        {
            string customerName;
            string claimID;
            DataTable dt = new DataTable("ExpenseTBL");
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));

            var context = _sqlExecuter.GetTLSDBContext();
            {
                //hvtam-23112014: Get customer by timesheetID
                customerName = (from t in context.TimeSheets
                                join c in context.Claims on t.ClaimID equals c.Id
                                join cus in context.Customers on c.InsurerID equals cus.Id
                                where t.Id == TimesheetID
                                select cus.BrandName != null && cus.BrandName != ""
                                           ? cus.BrandName
                                           : cus.Name).SingleOrDefault();
                //hvtam - 23112014
                claimID = context.TimeSheets.Where(c => c.Id == TimesheetID)
                                        .Select(c => c.ClaimID)
                                        .SingleOrDefault() ?? string.Empty;

                var query = from e in context.Expenses
                            where e.TimeSheetID == TimesheetID   //hvtam-23112014- e.Amount < 0 lam gi  where e.TimeSheetID == TimeSheetID && e.Amount < 0 
                            group e.Amount by e.ExpenseType.Name into g
                            select new
                            {
                                Description = g.Key ?? string.Empty,// e.Description ?? string.Empty,
                                Amount = (decimal?)g.Sum() ?? 0
                            };

                foreach (var i in query.ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr["Description"] = i.Description ?? string.Empty;
                    dr["Amount"] = Math.Abs(i.Amount);
                    dt.Rows.Add(dr);
                }
            }

            dynamic data = new ExpandoObject();
            data.CustomerName = customerName ?? string.Empty;
            data.claimID = claimID;
            data.Table = dt;
            return data;
        }
        //hvtam-23112014 GetExpenseDetailsByClaimID(int claimID) ->GetExpenseDetailsByTSID(int claimID)
        public dynamic GetExpenseDetailsByTimeSheetID(int TimesheetID)
        {
            string customerName;
            string claimID;
            DataTable dt = new DataTable("ExpenseTBL");
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("InputDate", typeof(DateTime));
            dt.Columns.Add("Employee", typeof(string));
            dt.Columns.Add("ExpenseType", typeof(string));

            var context = _sqlExecuter.GetTLSDBContext();
            {
                //hvtam-23112014: Get customer by timesheetID
                customerName = (from t in context.TimeSheets
                                join c in context.Claims on t.ClaimID equals c.Id
                                join cus in context.Customers on c.InsurerID equals cus.Id
                                where t.Id == TimesheetID
                                select cus.BrandName != null && cus.BrandName != ""
                                           ? cus.BrandName
                                           : cus.Name).SingleOrDefault();
                //hvtam - 23112014
                claimID = context.TimeSheets.Where(c => c.Id == TimesheetID)
                                        .Select(c => c.ClaimID)
                                        .SingleOrDefault() ?? string.Empty;
                var query = from e in context.Expenses
                            join emp in context.Employees on e.CreatedBy equals emp.Id //hvtam-23112014: Sau nay sua lai phai link qua timesheetID->claim->customer
                            where e.TimeSheetID == TimesheetID  //hvtam-23112014- e.Amount < 0 lam gi  where e.TimeSheetID == TimeSheetID && e.Amount < 0 
                            select new
                            {
                                Description = e.Description,// e.Description ?? string.Empty,
                                Amount = Math.Abs(e.Amount ?? 0),
                                InputDate = e.InputDate,
                                Employee = emp.Name,
                                ExpenseType = e.ExpenseType.Name
                            };

                foreach (var i in query.ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr["Description"] = i.Description ?? string.Empty;
                    dr["Amount"] = Math.Abs(i.Amount);
                    dr["InputDate"] = i.InputDate ?? DateTime.MinValue;
                    dr["Employee"] = i.Employee ?? string.Empty;
                    dr["ExpenseType"] = i.ExpenseType ?? string.Empty;
                    dt.Rows.Add(dr);
                }
            }

            dynamic data = new ExpandoObject();
            data.CustomerName = customerName ?? string.Empty;
            data.claimID = claimID ?? string.Empty;
            data.Table = dt;
            return data;
        }

        /// <summary>
        /// http://www.compilemode.com/2015/05/convert-linq-query-result-to-datatable.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Linqlist"></param>
        /// <returns></returns>
        public static DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();


            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        // Create, Delete, Update Type of Report combobox
        public List<ReportDbObj> GetAll()
        {
            List<ReportDbObj> list = _sqlExecuter.GetDatabase().SqlQuery<ReportDbObj>("Select * from Report").ToList();
            return list;
        }
        private int Create(ReportDbObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("INSERT INTO Report(Name) VALUES(@p0)", input.Name);
        }

        public int Delete(int ID)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("DELETE FROM Report WHERE ID = @p0", ID);
        }
        private int Update(ReportDbObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE Report SET Name=@p0 WHERE ID = @p1", input.Name, input.ID);
        }
        public int CreateOrUpdate(CreateOrUpdateReportInput input)
        {

            if (input.ID.HasValue)
            {
                ReportDbObj s = new ReportDbObj() { ID = input.ID.Value, Name = input.Name };
                return Update(s);
            }
            else
            {
                ReportDbObj s = new ReportDbObj() { Name = input.Name };
                return Create(s);
            }
        }

        public DataTable GetFeeByUser(DateTime fromDate, DateTime toDate, int empID, int issueType)
        {
            var context = _sqlExecuter.GetTLSDBContext();

            var curr_user = GetCurrentUser();
            var curr_role = "";
            var role_query = from r in context.Roles
                             join ur in context.UserRoles on r.Id equals ur.RoleId
                             where ur.UserId == curr_user.Id
                             select new RoleView
                             {
                                 ID = r.Id,
                                 Name = r.Name,
                                 DisplayName = r.DisplayName
                             };
            var roles = role_query.ToList();
            foreach (var role in roles)
            {
                curr_role = role.Name;
            }

            bool getAllFee = false;

            if (curr_role == "Admin" || checkPermission_AllActualFee())
            {
                getAllFee = true;
            }

            DataTable dt = new DataTable("FeeTBL");

            dt.Columns.Add("EmployeeName", typeof(string));
            //dt.Columns.Add("Hour", typeof(TimeSpan));
            //dt.Columns.Add("ApproveTime", typeof(TimeSpan)); //hvtam
            dt.Columns.Add("UserFee", typeof(decimal));
            dt.Columns.Add("TimesheetName", typeof(string));
            dt.Columns.Add("ExchangeRate", typeof(decimal));//hvtam-22042015
            dt.Columns.Add("TotalAmount", typeof(decimal));

            {
                DateTime startDT = fromDate.Date;
                DateTime endDT = toDate.Date.AddDays(1);
                var entries = from t in context.TimeSheets
                              join f in context.ProfessionalFees on t.Id equals f.TimeSheetID
                              join emp in context.Employees on f.CreateBy equals emp.Id
                              where f.InputDate != null
                                //hvtam-02122015 && t.CreateDate >= startDT  //hvtam-13113015 f.InputDate >= startDT
                                && f.InputDate >= startDT
                                && f.InputDate < endDT
                                && (((empID == 0) && (getAllFee || (emp.Id == curr_user.EmployeeId))) || emp.Id == empID)
                                && ((issueType == -1) || 
                                    ((issueType == 1) && (t.IssueDate >= startDT) && (t.IssueDate < endDT)) ||
                                    ((issueType == 0) && (((t.IssueDate < startDT) && (t.IssueDate >= endDT)) || (t.IssueDate == null)))
                                   )
                              //hvtam-13042015: report ca timesheet chua issue (WIPE) //&& (t.IsIssued ==true) //just report timesheet is issued
                              select new { f.CreateBy, f.ProFeeValue, emp.Name, f.FeePerHour, t.TimeSheetName, t.ExchangeRate } into x
                              //orderby f.InputDate
                              //group f by f.CreateBy into g
                              group x by new { x.Name, x.CreateBy, x.FeePerHour, x.TimeSheetName, x.ExchangeRate, } into g
                              select new
                              {
                                  EmployeeName = g.Key.Name,
                                  TimeSheetName = g.Key.TimeSheetName,
                                  ExchangeRate = g.Key.ExchangeRate,
                                  FeePerHour = g.Key.FeePerHour,
                                  TotalTsProFeeinUSDOfEmp = g.Sum(i => i.ProFeeValue)

                              };

                foreach (var i in entries.ToList())
                {
                    DataRow dr = dt.NewRow();
                    //dr["Date"] = TimeSpan.Zero;
                    dr["EmployeeName"] = i.EmployeeName ?? string.Empty;
                    //dr["Hour"] = TimeSpan.Zero;
                    dr["UserFee"] = i.FeePerHour ?? 0;
                    dr["TimesheetName"] = i.TimeSheetName ?? string.Empty;
                    //TimeSpan workingtime = (TimeSpan)(i.WorkTime ?? TimeSpan.Zero);
                    //int minute = workingtime.Hours * 60 + workingtime.Minutes;                    
                    //int TotalMinutes = TotalMinutes + minute;
                    dr["ExchangeRate"] = i.ExchangeRate ?? 0; //hvtam-22042015
                    dr["TotalAmount"] = i.TotalTsProFeeinUSDOfEmp * i.ExchangeRate; ////Tinh Profee theo VND = USD*Exchange
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        public dynamic GetExpense(DateTime fromDate, DateTime toDate, int empID)
        {
            DataTable dt = new DataTable("CashTBL");
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Debit", typeof(decimal));
            dt.Columns.Add("Credit", typeof(decimal));
            dt.Columns.Add("RefNbr", typeof(string));
            dt.Columns.Add("Notes", typeof(string));
            dt.Columns.Add("EmployeeName", typeof(string));

            var context = _sqlExecuter.GetTLSDBContext();
            {
                //openBal = context.Expenses.Where(c => c.CreatedDate != null && c.CreatedDate < fromDate.Date)
                //                          .Select(c => (decimal?)(c.Amount ?? 0))
                //                          .Sum() ?? 0;

                DateTime startDT = fromDate.Date;
                DateTime endDT = toDate.Date.AddDays(1);
                var entries = from e in context.Expenses
                              join emp in context.Employees on e.CreatedBy equals emp.Id
                              join ts in context.TimeSheets on e.TimeSheetID equals ts.Id //hvtam-28112014
                              where e.InputDate != null
                                && e.InputDate >= startDT //hvtam-02122015 && ts.CreateDate >= startDT     //hvtam-29112015 && e.CreatedDate >= startDT 
                                && e.InputDate < endDT    //hvtam-02122015 && ts.CreateDate < endDT        //hvtam-29112015 && e.CreatedDate < endDT
                                && (empID == 0 || emp.Id == empID)
                              select new
                              {
                                  Date = e.InputDate,
                                  e.Description,
                                  e.Amount,
                                  e.RefNbr,
                                  ts.TimeSheetName,
                                  e.Notes,
                                  EmployeeName = emp.Name
                              };

                foreach (var e in entries.ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr["Date"] = e.Date ?? DateTime.MinValue;
                    dr["Description"] = e.Description ?? string.Empty;
                    dr["Debit"] = 0;
                    dr["Credit"] = Math.Abs(e.Amount ?? 0);
                    //dr["Debit"] = (e.Amount > 0) ? e.Amount ?? 0 : 0;
                    //dr["Credit"] = (e.Amount < 0) ? e.Amount ?? 0 * (-1) : 0;
                    dr["RefNbr"] = string.IsNullOrWhiteSpace(e.RefNbr) ? e.TimeSheetName : e.RefNbr;
                    dr["Notes"] = e.Notes ?? string.Empty;
                    dr["EmployeeName"] = e.EmployeeName ?? "All";
                    dt.Rows.Add(dr);
                }
            }


            return dt;
        }

        public dynamic GetCashFlow(DateTime fromDate, DateTime toDate, string method)
        {
            //decimal openBal = 0;
            //decimal openBalNonClaim = 0; //hvtam-25112014 Cash from other payment/receipt (non claim)
            //decimal openBalClaim = 0; //hvtam-25112014 Cash receive from claim's earn
            //decimal openBalClaimCMS = 0; //hvtam-08122014 Cash payment for Commission
            DataTable dt = new DataTable("CashTBL");
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Debit", typeof(decimal));
            dt.Columns.Add("Credit", typeof(decimal));
            dt.Columns.Add("Method", typeof(string));

            var context = _sqlExecuter.GetTLSDBContext();
            {
                DateTime startDT = fromDate.Date;
                DateTime endDT = toDate.Date.AddDays(1);
                /*
                openBalNonClaim = (context.Cashs.Where(c => c.CreatedDate != null
                                                         && c.CreatedDate.Value < startDT
                                                         && (method == null || c.PaymentMethod == method)
                                                         && (c.IsDelete != true)   //hvtam-19022016 using isDelete Flag
                                                      )

                                                   .Sum(c => c.VoucherType == PAYMENT_TYPE_DEFINE.PAYMENT_TYPE_RECEIVE
                                                                ? c.Amount
                                                                : c.Amount * -1)) ?? 0;

                //hvtam-25112014
                openBalClaim = (context.ACT_Transactions.Where(c => c.PaymentDate != null //c => c.CreateDate
                                                         && c.PaymentDate.Value < startDT //c => c.CreateDate
                                                         && (method == null || c.PaymentMethod == method))
                                                    .Sum(c => c.PaymentAMT)) ?? 0;
                //hvtam-08122014
                openBalClaimCMS = (context.ACT_CommissionPayments.Where(c => c.PaymentDate != null   //c => c.CreateDate
                                                         && c.PaymentDate.Value < startDT           //c => c.CreateDate
                                                         && (method == null || c.PaymentMethod == method))
                                                    .Sum(c => c.PaymentAMT)) ?? 0;
                openBal = openBalNonClaim + openBalClaim + openBalClaimCMS; //balance */


                //hvtam-25112014: comment: get from nonclaim
                var entries = context.Cashs.Where(c => c.CreatedDate != null
                                                     && c.CreatedDate.Value >= startDT
                                                     && c.CreatedDate.Value < endDT
                                                     && (method == null || c.PaymentMethod == method)
                                                     && (c.IsDelete != true)   //hvtam-19022016 using isDelete Flag
                                                 )
                                            .Select(c => new
                                            {
                                                Date = c.CreatedDate,
                                                c.Description,
                                                c.VoucherType,
                                                c.Amount,
                                                c.PaymentMethod
                                            });

                foreach (var e in entries.ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr["Date"] = e.Date ?? DateTime.MinValue;
                    dr["Description"] = e.Description ?? string.Empty;
                    dr["Debit"] = (e.VoucherType == PAYMENT_TYPE_DEFINE.PAYMENT_TYPE_RECEIVE ? e.Amount ?? 0 : 0);
                    dr["Credit"] = (e.VoucherType == PAYMENT_TYPE_DEFINE.PAYMENT_TYPE_PAYMENT ? e.Amount ?? 0 : 0);
                    dr["Method"] = (e.PaymentMethod == PAYMENT_METHOD_DEFINE.PAYMENT_METHOD_CASH ? "Cash" : "TT");
                    dt.Rows.Add(dr);

                }

                //hvtam-25112014 Get Cash/bank from claim
                var queryclaim = from t in context.ACT_Transactions
                                 join iv in context.Invoices on t.InvoiceID equals iv.Id
                                 join cus in context.Customers on t.CustomerID equals cus.Id        //hvtam-08122015 fix display description report as Nguyen request
                                 where (t.PaymentDate != null && t.CurrentBalanceDebit != null                                     //hvtam-19022016 change CreateDate thanh PaymentDate: t.CreateDate
                                        && t.PaymentDate.Value >= startDT
                                        && t.PaymentDate.Value < endDT
                                        && (method == null || t.PaymentMethod == method)
                                        )
                                 select new
                                 {
                                     Date = t.PaymentDate,
                                     iv.InvoiceCode,
                                     iv.InvoiceDate,
                                     //invoiceDate =string.Format("{0:dd.MM.yyyy}", iv.InvoiceDate),
                                     t.RefCode,
                                     t.Remark,
                                     cus.BrandName,
                                     //t.PaymentType,
                                     t.PaymentAMT,
                                     t.PaymentMethod
                                 };

                foreach (var e in queryclaim.ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr["Date"] = e.Date ?? DateTime.MinValue;
                    dr["Description"] = "From " + e.BrandName + " payment of invoice No. " + e.InvoiceCode + " date " + ((DateTime)e.InvoiceDate).ToString("dd/MM/yyyy") + ", Ref. " + e.RefCode ?? string.Empty;   //hvtam-08122015 fix display description report as Nguyen request
                    //dr["Description"] = "From " + e.BrandName + " payment of invoice No. " + e.InvoiceCode + " date " + invoiceDate + ", Ref. " + e.RefCode ?? string.Empty;   //hvtam-08122015 fix display description report as Nguyen request
                    dr["Debit"] = e.PaymentAMT ?? 0;
                    dr["Credit"] = 0;
                    dr["Method"] = (e.PaymentMethod == PAYMENT_METHOD_DEFINE.PAYMENT_METHOD_CASH ? "Cash" : "TT");
                    dt.Rows.Add(dr);
                }

                //hvtam-08122014 Get Cash bank from Commission Payement
                var queryCMS = from t in context.ACT_CommissionPayments
                               join cms in context.Commissions on t.CommissionID equals cms.CommissionID
                               join iv in context.Invoices on cms.InvoiceID equals iv.Id
                               join cusoff in context.CustomerOfficers on cms.OfficerID equals cusoff.OfficerID
                               where (t.PaymentDate != null
                                              && t.PaymentDate.Value >= startDT
                                              && t.PaymentDate.Value < endDT
                                              && (method == null || t.PaymentMethod == method))
                               select new
                               {
                                   Date = t.PaymentDate,
                                   cusoff.FullName,
                                   iv.InvoiceCode,
                                   //t.PaymentType,
                                   t.PaymentAMT,
                                   t.PaymentMethod
                               };

                foreach (var e in queryCMS.ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr["Date"] = e.Date ?? DateTime.MinValue;
                    //dr["Description"] = e.InvoiceCode ?? string.Empty;
                    dr["Description"] = "CMS Payment " + e.InvoiceCode + " for " + e.FullName;
                    dr["Debit"] = 0;
                    dr["Credit"] = e.PaymentAMT ?? 0;
                    dr["Method"] = (e.PaymentMethod == PAYMENT_METHOD_DEFINE.PAYMENT_METHOD_CASH ? "Cash" : "TT");
                    dt.Rows.Add(dr);
                }



            } //End query


            //hvtam - 26112014: Sort by date
            DataView dv = new DataView(dt);
            dv.Sort = "Date";
            dt = dv.ToTable();
            /*
            dynamic data = new ExpandoObject();
            data.OpenBalance = openBal;
            data.Table = dt;*/
            return dt;
        }

        public DataTable GetWIPTimesheetsDate2Date(DateTime fromDate, DateTime toDate, string claimID, Boolean? isIssued, Boolean isInvoiced, int insurerID)
        {
            DataTable dt = new DataTable("RevenueWIPTBL");
            dt.Columns.Add("ClaimID", typeof(string));
            dt.Columns.Add("TimesheetName", typeof(string));
            dt.Columns.Add("Insurrer", typeof(string));
            dt.Columns.Add("Insurred", typeof(string));
            dt.Columns.Add("IssuedStatus", typeof(bool));
            dt.Columns.Add("NetFeeAMT", typeof(decimal));
            dt.Columns.Add("ExpenseAMT", typeof(decimal));

            var context = _sqlExecuter.GetTLSDBContext();

            DateTime startDT = fromDate.Date;
            if (toDate != DateTime.MaxValue.Date && toDate != null)
                toDate = toDate.Date.AddDays(1);//DateTime endDT = toDate.Date;
            var entries = from c in context.Claims
                          join t in context.TimeSheets on c.Id equals t.ClaimID
                          join cus in context.Customers on c.InsurerID equals cus.Id
                          where (claimID == "" || claimID == null || c.Id == claimID)
                                && ((isIssued == null)
                                    || ((isIssued.Value) && (t.IssueDate >= fromDate) && (t.IssueDate < toDate))
                                    || ((!isIssued.Value) && ((t.IssueDate == null) || (t.IssueDate < fromDate) || (t.IssueDate >= toDate)))
                                   )
                                && ((t.IsInvoiced == isInvoiced) || (t.InvoiceDate < fromDate) || (t.InvoiceDate >= toDate))
                                && (insurerID == -1 || c.InsurerID == insurerID)
                          orderby cus.BrandName, c.CreatedDate descending
                          select new
                          {
                              c.Id,
                              cus.BrandName,
                              c.TheInsured,
                              t.TimeSheetName,
                              t.ExchangeRate,
                              ExpenseAMT = context.Expenses.Where(exp => exp.TimeSheetID == t.Id && exp.InputDate >= fromDate && exp.InputDate < toDate).Sum(o => (decimal?)o.Amount) ?? 0,
                              ActualProFeeAMTUSD = context.ProfessionalFees.Where(pf => pf.TimeSheetID == t.Id && pf.InputDate >= fromDate && pf.InputDate < toDate).Sum(o => (decimal?)o.ProFeeValue) ?? 0 //hvtam-22042015  
                          };
            ////group f by f.CreateBy into g
            //group x by new { x.ID, x.BrandName, x.TheInsured, x.TimeSheetName, x.ExchangeRate, x.ProFeeAMTUSD,x.ExpenseAMT } into g
            //select new
            //{
            //    claimID = g.Key.ID,
            //    Insurerbrandname = g.Key.BrandName,
            //    Insured = g.Key.TheInsured,
            //    TimeSheetName = g.Key.TimeSheetName,
            //    ExchangeRate = g.Key.ExchangeRate,
            //    TotalTsProFeeinUSDOfTS = g.Key.pr,
            //    TotalTSExpense = g.Where(i => i.CreatedDate >= fromDate && i.CreatedDate <= toDate).Sum(i => i.Amount) ?? 0,

            //};

            foreach (var i in entries.ToList())
            {
                if (i.ActualProFeeAMTUSD != 0 || i.ExpenseAMT != 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["ClaimID"] = i.Id;
                    dr["TimesheetName"] = i.TimeSheetName ?? string.Empty;
                    //dr["ExchangeRate"] = i.ExchangeRate ?? 0; //hvtam-22042015
                    dr["NetFeeAMT"] = i.ActualProFeeAMTUSD * i.ExchangeRate; ////Tinh Profee theo VND = USD*Exchange
                    dr["ExpenseAMT"] = i.ExpenseAMT;
                    dr["Insurrer"] = i.BrandName;
                    dr["Insurred"] = i.TheInsured;
                    dt.Rows.Add(dr);
                }

            }
            return dt;


        }

        public DataTable GetACTRevenue(List<InvoiceView> InvoiceLST)
        {
            DataTable dt = new DataTable("RevenueTBL");
            dt.Columns.Add("Insurer", typeof(string));
            //dt.Columns.Add("Insured", typeof(string));
            dt.Columns.Add("ClaimID", typeof(string));
            dt.Columns.Add("InvoiceDate", typeof(DateTime));
            dt.Columns.Add("InvoiceCode", typeof(string));
            dt.Columns.Add("Fee", typeof(decimal));
            dt.Columns.Add("Expense", typeof(decimal));
            //dt.Columns.Add("Taxable", typeof(decimal));
            //dt.Columns.Add("Tax", typeof(decimal));
            dt.Columns.Add("TotalCost", typeof(decimal));     //Revenue                


            foreach (var item in InvoiceLST)
            {
                DataRow dr = dt.NewRow();
                dr["Insurer"] = item.CustomerBrandName ?? string.Empty; //Nên bỏ
                                                                        //dr["Insured"] = item.Insured ?? string.Empty;
                dr["ClaimID"] = item.ClaimID;// dr["RefNbr"] = string.IsNullOrWhiteSpace(item.RefNbr) ? item.ClaimID : item.RefNbr;
                dr["InvoiceDate"] = item.InvoiceDate ?? DateTime.MinValue;
                dr["InvoiceCode"] = item.InvoiceCode ?? string.Empty;
                dr["Fee"] = item.ProFeeGrandAMT;
                dr["Expense"] = item.ExpenseAMT; //test

                dt.Rows.Add(dr);
            }


            return dt;
        }

        public DataTable GetACT_ARREPORT(List<ARView> ARList)
        {
            DataTable dt = new DataTable("ARTBL");
            dt.Columns.Add("ClaimID", typeof(string));
            dt.Columns.Add("Insurer", typeof(string));
            dt.Columns.Add("InvoiceNo", typeof(string));
            dt.Columns.Add("Date", typeof(DateTime));
            //dt.Columns.Add("NetFee", typeof(decimal));
            //dt.Columns.Add("Expenses", typeof(decimal));
            dt.Columns.Add("SubTotal", typeof(decimal));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Paid", typeof(decimal));
            dt.Columns.Add("Balance", typeof(decimal));

            foreach (var item in ARList)
            {
                DataRow dr = dt.NewRow();
                dr["ClaimID"] = item.ClaimID ?? string.Empty;
                dr["Insurer"] = item.CustomerBranchName ?? string.Empty;

                dr["InvoiceNo"] = item.InvoiceCode ?? string.Empty;
                dr["Date"] = item.InvoiceDate;
                //dr["NetFee"] = item.ProFeeGrandAMT ?? 0;
                //dr["Expenses"] = item.ExpenseAMT ?? 0;
                dr["SubTotal"] = item.SubAMT ?? 0; //Amount->SubAMT
                dr["Total"] = item.TotalAMT ?? 0; //Amount->TotalAMT
                dr["Paid"] = item.PaidAMT ?? 0; //VAT-> Paid
                dr["Balance"] = item.RemainAMT ?? 0; //Total -> Remain
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public bool checkPermission_AllActualFee()
        {
            bool result = false;
            var curr_user = GetCurrentUser();
            var permission_searchclaim = PermissionManager.GetPermission(AppPermissions.Pages_ReportsExport_ActualFeeAll);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            result = permissionlist.Result.Contains(permission_searchclaim);
            return result;
        }

    }
}
