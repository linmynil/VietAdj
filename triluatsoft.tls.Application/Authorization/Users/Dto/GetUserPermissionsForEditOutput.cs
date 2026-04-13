using System.Collections.Generic;
using Abp.Application.Services.Dto;
using triluatsoft.tls.Authorization.Permissions.Dto;

namespace triluatsoft.tls.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}