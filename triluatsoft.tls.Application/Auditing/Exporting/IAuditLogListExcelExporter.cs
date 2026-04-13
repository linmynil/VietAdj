using System.Collections.Generic;
using triluatsoft.tls.Auditing.Dto;
using triluatsoft.tls.Dto;

namespace triluatsoft.tls.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);
    }
}
