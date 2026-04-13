using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.OldViewClass;
using triluatsoft.tls.OldTable.View;

namespace triluatsoft.tls.OldTable
{
    public interface ITimeSheetAppService: IApplicationService
    {
        int GetNewTSSeq(string claimId);
        string GetNewTSSeqStr(string claimId);
        void Create(string claimId, int userId);
        PagedResultDto<VTimesheet> SearchTimeSheet(TimeSheetSearchOption opt);
        List<ProfessionalFeeView> GetProfessionalFee(int timeSheetID);
        string SaveFee(CreateOrUpdateFeeInput input);
        string DeleteFee(int ProfessionalFeeID);
        List<ExpenseView> GetUserExpense(int TimeSheetID);
        string SaveExpense(CreateOrUpdateExpenseInput input);
        string DeleteExpense(int expenseID);
        string Issued(int TimeSheetID, DateTime IssueDate);
        string UnIssued(int TimeSheetID);
        TimeSheetView GetTSInfo(int TimeSheetID);
        TimeSheetView GetTSInfoByEmp(int TimeSheetID, int EmployeeID);
        string UpdateTimesheet(UpdateTimesheetInput input);
        string CreateTimesheet(CreateTimesheetInput input);
        string DeleteTimeSheet(int TimeSheetID);
        List<TimeSheetView> GetTimesheetToCreateInvoice(string claimID);
        List<TimeSheetView> GetListByInvoiceID(int InvoiceID);
        List<TimeSheetView> GetListApproved();
        List<VClaimInsurerDto> GetListSubmission();
        PagedResultDto<TimeSheetView> SearchTimeSheet2(TimeSheetSearchOption opt);
        //20180815
        string IsSubmit(int TimeSheetID);
        string UnIsSubmit(int TimeSheetID);
        bool checkPermission_AMManagementTimeSheet();
        bool checkPermission_MyManagementTimeSheet();
        bool checkPermission_AllManagementTimeSheet();
        bool checkPermission_CreateTimeSheet();
        bool checkPermission_EnterTimeSheet();
        bool checkPermission_IssueTimeSheet();
        bool checkPermission_DeleteTimeSheet();
        bool checkPermission_EditTimeSheet();
        bool checkPermission_TransferTimeSheet();
        bool checkPermission_SubmitTimeSheet();
        bool checkPermission_InvoiceTimeSheet();
        bool checkPermission_IssuedTimeSheetOnly();
        bool checkPermission_NotIssuedTimeSheet();        
        string TransferTimesheet(UpdateTimesheetInput input);
    }
}
