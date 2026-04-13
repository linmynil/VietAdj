using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Dto;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.OldViewClass;

namespace triluatsoft.tls.OldTable
{
    public interface IClaimAppService: IApplicationService
    {
        PagedResultDto<VClaimInsurerDto> GetAll(SearchClaimInput input);
        VClaimInsurerDto GetById(string Id);
        string Create(CreateOrUpdateClaimInput input);
        BorderauxView GetBorderauxInfo(string claimID);
        List<string> GetOpenClaim();
        List<string> GetOpenClaimToBorderaux();
        List<ReportView> GetListReport();
        List<BordereauxStatusView> GetActiveBordereauxStatus();
        List<FollowUpView> GetFollowUpList();
        List<BorderauxView> GetBordereauxHistories(string claimID);
        string CreateBorderaux(UpdateBorderauxStatusInput input);
        List<string> GetOpenClaimToCreateTimesheet();
        List<string> GetOpenClaimToCreateTask();
        List<EmployeeView> GetEmployeeByClaim(string claimID);
        List<string> GetOpenClaimToCreateInvoice();
        int CreateClaimDirectory(string foldername, int claimSTT);
        List<Subfolders> GetSubfoldersList();
        List<OutStandingView> GetTotalOutStanding();
        List<ClaimView> GetClaimsByEmployee(int rtype);
        bool checkPermission_SearchClaim();
        bool checkPermission_CreateClaim();
        bool checkPermission_EditClaim();
        bool checkPermission_AssignClaim();
        bool checkPermission_UpdateStatusClaim();
        bool checkPermission_AllClaim();
        FileDto ExporClaimFunc(ExportClaimInput input);
    }
}
