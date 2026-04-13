using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.OldViewClass;
using triluatsoft.tls.OldTable.View;

namespace triluatsoft.tls.OldTable
{
    public interface IDashBoardAppService : IApplicationService
    {
        List<BorderauxView> GetAll();
        List<NotificationView> LoadNotifications();

        bool isAdminCurrentUser();

    }
}
