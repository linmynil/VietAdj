using System.Collections.Generic;
using triluatsoft.tls.Authorization.Users.Dto;
using triluatsoft.tls.Dto;

namespace triluatsoft.tls.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}