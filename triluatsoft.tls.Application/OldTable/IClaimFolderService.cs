using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;
using System.Web.UI.WebControls;
using triluatsoft.tls.Dto;

namespace triluatsoft.tls.OldTable
{
    public interface IClaimFolderService : IApplicationService
    {
        FolderDetails GetFolderDetails(string virtualPath);
        FileDto DownloadClaimFile(string fName, string virtualPath);
        List<ListFolder> GetFolderList(string virtualPath);
        List<ListFiles> GetFilesList(string virtualPath);
        PagedResultDto<ClaimFolderInfo> GetAll(FolderSearchOption input);
        List<ClaimFolderInfo> GetUploadFolderList();
        List<ClaimFolderInfo> SearchUploadFolderList(LibreryUploadSearchOptions opt);
        FileDto DownloadLabFile(string fName);
    }
}
