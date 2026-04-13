using Abp.Authorization;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Dynamic;
using System.Web.Mvc;
using triluatsoft.tls.Authorization;
using triluatsoft.tls.CrystalReports;
using triluatsoft.tls.Dto;
using triluatsoft.tls.OldUtils;
using triluatsoft.tls.Datasets;
using System;
using triluatsoft.tls.EntityFramework;
using System.Linq;
using triluatsoft.tls.OldTable.OldViewClass;
using triluatsoft.tls.OldTable.dto;
using System.Collections.Generic;

namespace triluatsoft.tls.Web.Controllers
{
    public class ReportController : tlsControllerBase
    {
        private readonly IReportAppService _reportAppService;
        private readonly OldTable.ICustomerAppService _customerService;
        private readonly OldTable.IEmployeeAppService _empService;
        private readonly OldTable.ITimeSheetAppService _timesheetService;
        private readonly ISqlExecuter _sqlExecuter;
        private readonly OldTable.IInvoiceAppService _invoiceService;
        private readonly OldTable.IARAppService _aRService;



        public ReportController(IReportAppService reportAppService
            , OldTable.ICustomerAppService customerService, OldTable.IEmployeeAppService empService
            , OldTable.ITimeSheetAppService timesheetService
            , ISqlExecuter sqlExecuter,
            OldTable.IInvoiceAppService invoiceService,
            OldTable.IARAppService aRService
            )
        {
            _reportAppService = reportAppService;
            _customerService = customerService;
            _empService = empService;
            _timesheetService = timesheetService;
            _sqlExecuter = sqlExecuter;
            _invoiceService = invoiceService;
            _aRService = aRService;
        }


        //TODO CHECK PERMISSION
        //[AbpAuthorize(AppPermissions.Pages_Administration_Users)]
        public ActionResult Index(string claimId, string lang)
        {
            Logger.Debug("claimId " + claimId + " " + lang);
            dynamic data = new ExpandoObject();
            var dsource = _reportAppService.ReportTimeSheetSum(claimId, ref data);
            string Path = string.Format("~/Reports/rptTimeSheet_001_{0}.rpt", lang);
            string rptPath = Server.MapPath(Path);
            ReportDocument rd = new ReportDocument();
            Session["report"] = rd;
            rd.Load(rptPath);
            rd.SetDataSource(dsource);
            rd.SetParameterValue("ClaimID", data.ClaimID);
            rd.SetParameterValue("CompanyName", data.TheInsured);
            rd.SetParameterValue("ExchangeRate", data.ExchangeRate);
            rd.SetParameterValue("ProFeeAMT", data.ProFeeGrandAMT==null?0:data.ProFeeGrandAMT);
            rd.SetParameterValue("ExpenseAMT", data.ExpenseAMT==null?0:data.ExpenseAMT);
            rd.SetParameterValue("TotalAMT", data.TotalAMT==null?0:data.TotalAMT);
            rd.SetParameterValue("TaxAMT", data.TaxAMT==null?0:data.TaxAMT);
            rd.SetParameterValue("GrandAMT", data.GrandAMT==null?0:data.GrandAMT);
            rd.SetParameterValue("RemainAMT", data.RemainAMT==null?0:data.RemainAMT);
            if (lang == "VN")
            {
                rd.SetParameterValue("RemainAMTText", CNum2Char.So2Chu(data.RemainAMT));//Doc so thanh chu, 15 -> muoi lam
            } else
            {
                rd.SetParameterValue("RemainAMTText", CNum2Char.Num2Words(data.RemainAMT));//Doc so thanh chu, 15 -> muoi lam
            }
            
            return Redirect("~/reports/viewreport.aspx");
        }


        //TODO CHECK PERMISSION
        //[AbpAuthorize(AppPermissions.Pages_Administration_Users)]
        public ActionResult ClaimBorderauxReport(int custID, string lang="VN")
        {
            string nameposition;
            Logger.Debug("ClaimBorderauxReport custID " + custID + " " + lang);
          
            ClaimBordereauxDS ds = new ClaimBordereauxDS(); //Ten cua ClaimBordereauxDS.xsd
            DataTable dt = new DataTable();
            dt.TableName = "Claim Borderaux";
            dt = _reportAppService.GetBordereauxReportNEW(custID);
            var custView = _customerService.GetInfo(custID);
            ds.Tables[0].Merge(dt);

            string Path = string.Format("~/Reports/ClaimBordereauxReport_{0}.rpt", lang);
            string rptPath = Server.MapPath(Path);

            ReportDocument rd = new ReportDocument();            
            Session["report"] = rd;            
            rd.Load(rptPath);
            rd.SetDataSource(ds);
            rd.OpenSubreport("StatusListSubReport.rpt")
              .SetDataSource(_reportAppService.GetBordereauxStatusList());

            if (custView.ContactName == null || custView.ContactName=="" || custView.ContactName=="N/A" || custView.ContactName == "Unknown")
            {
                nameposition = "-";
            }
            else {
                nameposition = (custView.ContactPosition == null || custView.ContactPosition == "") ? custView.ContactName : custView.ContactName + " - " + custView.ContactPosition ;
            }         
            
            rd.SetParameterValue("CustomerName", custView.CustomerName);
            rd.SetParameterValue("ContactName", nameposition);
            //rd.SetParameterValue("Website", "web");
            //rd.SetParameterValue("Address1", "add1");
            //rd.SetParameterValue("Address2", "add2");
            //rd.SetParameterValue("Telephone", "tel");
            //rd.SetParameterValue("Fax", "fax");

            rd.SetParameterValue("Website", ConfigHelper.GetString(ConfigKeys.COMPANY_WEBSITE));
            rd.SetParameterValue("Address1", ConfigHelper.GetString(ConfigKeys.COMPANY_ADDRESS1));
            rd.SetParameterValue("Address2", ConfigHelper.GetString(ConfigKeys.COMPANY_ADDRESS2));
            rd.SetParameterValue("Telephone", ConfigHelper.GetString(ConfigKeys.COMPANY_TEL));
            rd.SetParameterValue("Fax", ConfigHelper.GetString(ConfigKeys.COMPANY_FAX));
            

            return Redirect("~/reports/viewreport.aspx");
        }

        //TODO CHECK PERMISSION
        //[AbpAuthorize(AppPermissions.Pages_Administration_Users)]
        public ActionResult TimesheetFeeReport(int tsID, int empID, string lang="EN")
        {

            OldTable.EmployeeView emp = _empService.GetEmpInfo(empID);
            string reportEmpName = emp.Name + " - " + emp.JobTitle + "," + emp.JobPosition;
            OldTable.OldViewClass.TimeSheetView TSView = _timesheetService.GetTSInfo(tsID);
                
            if (TSView == null)
                return null;

            dynamic data = _reportAppService.GetChargedProfeeOfTSByUser(tsID, empID, TSView.ExchangeRate ?? 1);
            //TODO
            //if (HasPermission(PageCapability.Timesheet_Manage_Admin)) //admin right: Report Charged profee
            //{
            
            string Path = string.Format("~/Reports/TimeSheetReport_{0}.rpt", lang);
            string rptPath = Server.MapPath(Path);
            ReportDocument rd = new ReportDocument();
            Session["report"] = rd;
            rd.Load(rptPath);
            rd.SetDataSource(data.Table);
            

                rd.SetParameterValue("RefNbr", TSView.TimeSheetName); //hvtam-21112014           
                rd.SetParameterValue("UserName", reportEmpName);
                rd.SetParameterValue("ExchangeRate", TSView.ExchangeRate);//hvtam-25122014 rd.SetParameterValue("ExchangeRate", data.ExchangeRate);
                rd.SetParameterValue("Fee", data.UserFee);
                rd.SetParameterValue("TotalWorkTime", data.TotalWorkTime);  //hvtam-21112014
                rd.SetParameterValue("TotalAmount", data.TotalAmount);      //hvtam-21112014
            

            //}
            //else if (HasPermission(PageCapability.Timesheet_Report_ProFee)) //user thuong, report Actual Profee
            //{

            //    //dynamic data = CrystalReportServiceUOW.GetActualProfeeOfTSByUser(TimesheetID, empID, TSView.ExchangeRate ?? 1);
            //    ReportDocument rd = new ReportDocument();
            //    rd.Load(GetReportFilePathbyLanague(rptlanguage));//hvtam-22112014
            //    rd.SetDataSource(data.Table);
            //    rd.SetParameterValue("RefNbr", TSView.TimeSheetName); //hvtam-21112014           
            //    rd.SetParameterValue("UserName", reportEmpName);
            //    rd.SetParameterValue("ExchangeRate", TSView.ExchangeRate);//hvtam-25122014 rd.SetParameterValue("ExchangeRate", data.ExchangeRate);
            //    rd.SetParameterValue("Fee", data.UserFee);
            //    rd.SetParameterValue("TotalWorkTime", data.TotalWorkTime);  //hvtam-21112014
            //    rd.SetParameterValue("TotalAmount", data.TotalAmount);      //hvtam-21112014

            //    TimeSheetReportViewer.ReportSource = rd;
            //    TimeSheetReportViewer.Zoom(75);
            //    ReportSource = rd; //hvtam-13032015
            //}
            return Redirect("~/reports/viewreport.aspx");

        }

        //TODO CHECK PERMISSION
        //[AbpAuthorize(AppPermissions.Pages_Administration_Users)]
        public ActionResult TimesheetExpenseReport(int tsID, string lang = "EN", string type = "default")
        {
            
            OldTable.OldViewClass.TimeSheetView TSView = _timesheetService.GetTSInfo(tsID);

            if (TSView == null)
                return null;


            ReportDocument rd = new ReportDocument();
            Session["report"] = rd;
            string Path = "";
            dynamic data = null;
            if (type == "detail")
            {
                Path = string.Format("~/Reports/ExpenseByClaimDetailReport_{0}.rpt", lang);
                data = _reportAppService.GetExpenseDetailsByTimeSheetID(tsID);
            }
            else
            {
                Path = string.Format("~/Reports/ExpenseByClaimReport_{0}.rpt", lang);
                data = _reportAppService.GetExpensesListByTimeSheetID(tsID);
            }
            string rptPath = Server.MapPath(Path);
            rd.Load(rptPath);

            rd.SetDataSource(data.Table);
            rd.SetParameterValue("RefNbr", TSView.TimeSheetName);
            rd.SetParameterValue("Insurer", data.CustomerName);

            return Redirect("~/reports/viewreport.aspx");

        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_Users)]
        public ActionResult ActualFeeReport(int empID, string txtFromDate1, string txtToDate1, string lang, int issueType)
        {
            DateTime txtFromDate = Convert.ToDateTime(txtFromDate1);
            DateTime txtToDate = Convert.ToDateTime(txtToDate1);

            DateTime? fromDate = txtFromDate;
            if (fromDate == null)
            {
                //this.Alert(Messages.GetMessage(MessageKeys.SELECT_FROM_DATE));
                //return;
            }

            DateTime? toDate = txtToDate;
            if (toDate == null)
                toDate = DateTime.Now.Date;

            if (toDate.Value.Date < fromDate.Value.Date)
            {
                //this.Alert(Messages.GetMessage(MessageKeys.FROM_DATE_GREATER_TO_DATE));
                //return;
            }

            //int employeeID = Convert.ToInt32(UserDropDown.SelectedValue);
            int? employeeID = empID;
            
            if (employeeID == null)
            {
                employeeID = 0;
            }
           

            //string employeeName = UserDropDown.SelectedItem.Text;

            var data = _reportAppService.GetFeeByUser(fromDate.Value, toDate.Value, employeeID.Value, issueType);

            string Path = string.Format("~/Reports/AppraisalFeeReport_{0}.rpt", lang);
            string rptPath = Server.MapPath(Path);
            ReportDocument rd = new ReportDocument();
            Session["report"] = rd;
            rd.Load(rptPath);
            rd.SetDataSource(data);

            if (empID == 0)
            {
                rd.SetParameterValue("Employeename", "All");
            }
            else {
                var em = _empService.GetEmpInfo(employeeID.Value);
                rd.SetParameterValue("Employeename", em.Name);
            }
            rd.SetParameterValue("FromDate", fromDate.Value);
            rd.SetParameterValue("ToDate", toDate.Value);

            return Redirect("~/reports/viewreport.aspx");
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_Users)]
        public ActionResult MonthlyExpenseReport(int empID, string txtFromDate1, string txtToDate1, string lang)
        {
            DateTime txtFromDate = Convert.ToDateTime(txtFromDate1);
            DateTime txtToDate = Convert.ToDateTime(txtToDate1);

            DateTime? fromDate = txtFromDate;
            if (fromDate == null)
            {
                //this.Alert(Messages.GetMessage(MessageKeys.SELECT_FROM_DATE));
                //return;
            }

            DateTime? toDate = txtToDate;
            if (toDate == null)
                toDate = DateTime.Now.Date;

            if (toDate.Value.Date < fromDate.Value.Date)
            {
                //this.Alert(Messages.GetMessage(MessageKeys.FROM_DATE_GREATER_TO_DATE));
                //return;
            }

            //int employeeID = Convert.ToInt32(UserDropDown.SelectedValue);
            int? employeeID = empID;
            if (employeeID == null)
            {
                employeeID = 0;
            }

            //string employeeName = UserDropDown.SelectedItem.Text;

            var data = _reportAppService.GetExpense(fromDate.Value, toDate.Value, employeeID.Value);

            string Path = string.Format("~/Reports/ExpenseReport_{0}.rpt", lang);
            string rptPath = Server.MapPath(Path);
            ReportDocument rd = new ReportDocument();
            Session["report"] = rd;
            rd.Load(rptPath);
            rd.SetDataSource(data);

            rd.SetParameterValue("FromDate", fromDate.Value);
            rd.SetParameterValue("ToDate", toDate.Value);

            return Redirect("~/reports/viewreport.aspx");
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_Users)]
        public ActionResult CashReport(string txtFromDate1, string txtToDate1, string method, string reportname, string lang)
        {
            //khong truyen method, reportname
            DateTime txtFromDate = Convert.ToDateTime(txtFromDate1);
            DateTime txtToDate = Convert.ToDateTime(txtToDate1);
            decimal openBal = 0;
            decimal openBalNonClaim = 0; //hvtam-25112014 Cash from other payment/receipt (non claim)
            decimal openBalClaim = 0; //hvtam-25112014 Cash receive from claim's earn
            decimal openBalClaimCMS = 0; //hvtam-08122014 Cash payment for Commission
            var context = _sqlExecuter.GetTLSDBContext();
            DateTime? fromDate = txtFromDate;

            if (fromDate == null)
            {
                fromDate = new DateTime(2017, 1, 1);
            }

            DateTime? toDate = txtToDate;
            if (toDate == null)
                toDate = DateTime.Now.Date;

            if (toDate.Value.Date < fromDate.Value.Date)
            {
                //this.Alert(Messages.GetMessage(MessageKeys.FROM_DATE_GREATER_TO_DATE));
                //return;
            }

            //int employeeID = Convert.ToInt32(UserDropDown.SelectedValue);


            //string employeeName = UserDropDown.SelectedItem.Text;

            openBalNonClaim = (context.Cashs.Where(c => c.CreatedDate != null
                                                         && c.CreatedDate.Value < fromDate
                                                         && (method == null || c.PaymentMethod == method)
                                                         && (c.IsDelete != true)   //hvtam-19022016 using isDelete Flag
                                                      )

                                                   .Sum(c => c.VoucherType == PAYMENT_TYPE_DEFINE.PAYMENT_TYPE_RECEIVE
                                                                ? c.Amount
                                                                : c.Amount * -1)) ?? 0;

            //hvtam-25112014
            openBalClaim = (context.ACT_Transactions.Where(c => c.PaymentDate != null && c.CurrentBalanceDebit != null //c => c.CreateDate
                                                     && c.PaymentDate.Value < fromDate //c => c.CreateDate
                                                     && (method == null || c.PaymentMethod == method))
                                                .Sum(c => c.PaymentAMT)) ?? 0;
            //hvtam-08122014
            openBalClaimCMS = (context.ACT_CommissionPayments.Where(c => c.PaymentDate != null   //c => c.CreateDate
                                                     && c.PaymentDate.Value < fromDate           //c => c.CreateDate
                                                     && (method == null || c.PaymentMethod == method))
                                                .Sum(c => c.PaymentAMT)) ?? 0;

            openBal = openBalNonClaim + openBalClaim - openBalClaimCMS;
            

            var data = _reportAppService.GetCashFlow(fromDate.Value, toDate.Value, method);

            string Path = string.Format("~/Reports/{1}{0}.rpt", lang, reportname);
            string rptPath = Server.MapPath(Path);
            ReportDocument rd = new ReportDocument();
            Session["report"] = rd;
            rd.Load(rptPath);
            rd.SetDataSource(data);

            rd.SetParameterValue("OpenBalance", openBal);
            rd.SetParameterValue("FromDate", fromDate.Value);
            rd.SetParameterValue("ToDate", toDate.Value);

            return Redirect("~/reports/viewreport.aspx");
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_Users)]
        public ActionResult WIPReport(WIPReport vm)
        {
            string reportname;
            string ClaimID = vm.txtClaimID;
            DateTime? fromDate = vm.txtFromDate;

            if (fromDate == null)
                fromDate = new DateTime(2017, 1, 1);
            else
                fromDate = fromDate.Value.Date;

            DateTime? toDate = vm.txtToDate;
            if (toDate == null)
                toDate = DateTime.Now;
            //else
            //    toDate = toDate.Value.Date.AddDays(1);

            Boolean? isIssued = false;
            Boolean isInvoiced = false;

            int insurerID = string.IsNullOrEmpty(vm.insurer) ? -1 : int.Parse(vm.insurer);
            //if (String.IsNullOrEmpty(ClaimID) && (vm.txtFromDate > vm.txtToDate))
            //{

                //this.Alert(Messages.GetMessage(MessageKeys.FROM_DATE_GREATER_TO_DATE));
                //return;

            //}

            RevenueWIPDS ds = new RevenueWIPDS(); //Ten cua RevenueWIPDS.xsd
            DataTable dtinvoice = new DataTable();
            DataTable dt = new DataTable();
            dt.TableName = "WIPTable";            
            switch (vm.rdolisIssued)
            {
                case "0": //WIP1
                    reportname = "WIP 1 REPORT";
                    isIssued = false;
                    isInvoiced = false;
                    dt = _reportAppService.GetWIPTimesheetsDate2Date(fromDate.Value, toDate.Value, ClaimID, isIssued, isInvoiced, insurerID);
                    break;
                case "1": //WIP2
                    reportname = "WIP 2 REPORT";
                    isIssued = true;
                    isInvoiced = false;
                    dt = _reportAppService.GetWIPTimesheetsDate2Date(fromDate.Value, toDate.Value, ClaimID, isIssued, isInvoiced, insurerID);
                    break;
                default: //ALL (WIP1 + WIP2)
                    reportname = "ALL WIP REPORT";
                    isIssued = null;
                    isInvoiced = false;
                    dt = _reportAppService.GetWIPTimesheetsDate2Date(fromDate.Value, toDate.Value, ClaimID, isIssued, isInvoiced, insurerID);
                    break;
            }

            ds.Tables[0].Merge(dt);
            string Path = string.Format("~/Reports/RevenueWIPReport_{0}.rpt", vm.lang);
            string rptPath = Server.MapPath(Path);
            ReportDocument rd = new ReportDocument();
            Session["report"] = rd;
            rd.Load(rptPath);
            rd.SetDataSource(dt);

            rd.SetParameterValue("REPORTNAME", reportname);
            rd.SetParameterValue("FromDate", fromDate.Value);
            rd.SetParameterValue("ToDate", toDate.Value);

            return Redirect("~/reports/viewreport.aspx");
        }
        
        public ActionResult RevenueReport(InvoiceSearchOption iNVOption)
        {
            
           
            //if (iNVOption.FromDate == null || iNVOption.ToDate == null)
            //{
            //    this.Alert(Messages.GetMessage(MessageKeys.SELECT_FROM_DATE));
            //    return;
            //}

            //if (iNVOption.FromDate > iNVOption.ToDate)
            //{
            //    this.Alert(Messages.GetMessage(MessageKeys.FROM_DATE_GREATER_TO_DATE));
            //    return;
            //}

            List<InvoiceView> SearchInvoiceLST =  _invoiceService.SearchReport(iNVOption);
            RevenueDS ds = new RevenueDS(); //Ten cua RevenueDS.xsd
            DataTable dtinvoice = new DataTable();
            DataTable dt = new DataTable();
            dt.TableName = "Invoice Table";
            dt = _reportAppService.GetACTRevenue(SearchInvoiceLST); //Retrieve data to display
            ds.Tables[0].Merge(dt);    
            
            string Path = string.Format("~/Reports/RevenueReport_{0}.rpt", iNVOption.lang);
            string rptPath = Server.MapPath(Path);
            ReportDocument rd = new ReportDocument();
            Session["report"] = rd;
            rd.Load(rptPath);
            rd.SetDataSource(dt);
            if (iNVOption.IsAdvInvoice == false )
            {
                iNVOption.InvoiceType = "TIMESHEET";
            }
            else if (iNVOption.IsAdvInvoice == true)
            {
                iNVOption.InvoiceType = "INTERIM";
            }
            else {
                iNVOption.InvoiceType = "ALL";
            }

            rd.SetParameterValue("InvoiceType", iNVOption.InvoiceType); //hvtam-01052016

            rd.SetParameterValue("FromDate", iNVOption.FromDate);
            rd.SetParameterValue("ToDate", iNVOption.ToDate);

            return Redirect("~/reports/viewreport.aspx");
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_Users)]
        public ActionResult ARReport(InvoiceSearchOption aRSearchOption)
        {

            //if (aRSearchOption.FromDate == null || aRSearchOption.ToDate == null)
            //{
            //    this.Alert(Messages.GetMessage(MessageKeys.SELECT_FROM_DATE));
            //    return;
            //}

            //if (aRSearchOption.FromDate > aRSearchOption.ToDate)
            //{
            //    this.Alert(Messages.GetMessage(MessageKeys.FROM_DATE_GREATER_TO_DATE));
            //    return;
            //}

            ARDS ds = new ARDS(); //Ten cua RevenueDS.xsd
            DataTable dtAR = new DataTable();
            DataTable dt = new DataTable();
            dt.TableName = "AR Table";
            List<ARView> ARListSearchResult = _aRService.SearchReport(aRSearchOption);
            dt = _reportAppService.GetACT_ARREPORT(ARListSearchResult); //Retrieve data to display
            ds.Tables[0].Merge(dt);
            

            string Path = string.Format("~/Reports/AccountsReceivableReport_{0}.rpt", aRSearchOption.lang);
            string rptPath = Server.MapPath(Path);
            ReportDocument rd = new ReportDocument();
            Session["report"] = rd;
            rd.Load(rptPath);
            rd.SetDataSource(ds);
            if (aRSearchOption.IsAdvInvoice == false)
            {
                aRSearchOption.InvoiceType = "TIMESHEET ";
            }
            else if (aRSearchOption.IsAdvInvoice == true)
            {
                aRSearchOption.InvoiceType = "INTERIM ";
            }
            else
            {
                aRSearchOption.InvoiceType = "ALL ";
            }

            rd.SetParameterValue("FromDate", aRSearchOption.FromDate.Value);
            rd.SetParameterValue("ToDate", aRSearchOption.ToDate.Value);
            rd.SetParameterValue("InvoiceType", aRSearchOption.InvoiceType);

            return Redirect("~/reports/viewreport.aspx");
        }
    }
}