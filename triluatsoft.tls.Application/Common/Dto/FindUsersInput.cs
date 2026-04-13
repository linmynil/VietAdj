using triluatsoft.tls.Dto;

namespace triluatsoft.tls.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}