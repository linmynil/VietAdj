using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Dto;
using triluatsoft.tls.OldTable;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.OldViewClass;

namespace triluatsoft.tls.CrystalReports
{
    public interface IReportAppService : IApplicationService
    {
        DataTable ReportTimeSheetSum(string ClaimID, ref dynamic ClaimSummary);
        DataTable GetBordereauxReportNEW(int insurerID);
        DataTable GetBordereauxStatusList();
        dynamic GetChargedProfeeOfTSByUser(int TimesheetID, int empID, decimal exchRate);
        FileDto ExportTimesheetFeeReport(int tsID, string lang = "EN");
        dynamic GetExpensesListByTimeSheetID(int TimesheetID);
        dynamic GetExpenseDetailsByTimeSheetID(int TimesheetID);
        //Report WIP, Revenue, Actual Fee, Contribution, Expense Monthly,Cash on Hand, Cash in Bank
        DataTable GetFeeByUser(DateTime fromDate, DateTime toDate, int empID, int issueType);
        dynamic GetExpense(DateTime fromDate, DateTime toDate, int empID);
        dynamic GetCashFlow(DateTime fromDate, DateTime toDate, string method);
        DataTable GetWIPTimesheetsDate2Date(DateTime fromDate, DateTime toDate, string claimID, Boolean? isIssued, Boolean isInvoiced, int insurerID);
        DataTable GetACTRevenue(List<InvoiceView> InvoiceLST);
        DataTable GetACT_ARREPORT(List<ARView> ARList);
        //Create, Update,Delete Type of Report combobox
        List<ReportDbObj> GetAll();
        int Delete(int ID);
        int CreateOrUpdate(CreateOrUpdateReportInput input);
        bool checkPermission_AllActualFee();
    }
}
