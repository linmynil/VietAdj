using Abp.Application.Navigation;
using Abp.Localization;
using triluatsoft.tls.Authorization;
using triluatsoft.tls.Web.Navigation;

namespace triluatsoft.tls.Web.App.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See .cshtml and .js files under App/Main/views/layout/header to know how to render menu.
    /// </summary>
    public class AppNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Tenants,
                    L("Tenants"),
                    url: "host.tenants",
                    icon: "icon-globe",
                    requiredPermissionName: AppPermissions.Pages_Tenants
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Editions,
                    L("Editions"),
                    url: "host.editions",
                    icon: "icon-grid",
                    requiredPermissionName: AppPermissions.Pages_Editions
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Tenant.Dashboard,
                    L("Dashboard"),
                    url: "tenant.dashboard",
                    icon: "icon-home",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Dashboard
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Administration,
                    L("Administration"),
                    icon: "icon-wrench"
                    )
                    //.AddItem(new MenuItemDefinition(
                    //    PageNames.App.Common.OrganizationUnits,
                    //    url: "organizationUnits",
                    //    icon: "icon-layers",
                    //    requiredPermissionName: AppPermissions.Pages_Administration_OrganizationUnits
                    //    )
                    //)
                    .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Roles,
                        L("Roles"),
                        url: "roles",
                        icon: "icon-briefcase",
                        requiredPermissionName: AppPermissions.Pages_Administration_Roles
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Users,
                        L("Users"),
                        url: "users",
                        icon: "icon-users",
                        requiredPermissionName: AppPermissions.Pages_Administration_Users
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Languages,
                        L("Languages"),
                        url: "languages",
                        icon: "icon-flag",
                        requiredPermissionName: AppPermissions.Pages_Administration_Languages
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.AuditLogs,
                        L("AuditLogs"),
                        url: "auditLogs",
                        icon: "icon-lock",
                        requiredPermissionName: AppPermissions.Pages_Administration_AuditLogs
                        )
                    )
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Maintenance,
                    L("Maintenance"),
                    url: "host.maintenance",
                    icon: "icon-wrench",
                    requiredPermissionName: AppPermissions.Pages_Administration_Host_Maintenance
                    )
                )
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Settings,
                    L("Settings"),
                    url: "host.settings",
                    icon: "icon-settings",
                    requiredPermissionName: AppPermissions.Pages_Administration_Host_Settings
                    )
                )
                //.AddItem(new MenuItemDefinition(
                //    PageNames.App.Tenant.Settings,
                //    L("Settings"),
                //    url: "tenant.settings",
                //    icon: "icon-settings",
                //    requiredPermissionName: AppPermissions.Pages_Administration_Tenant_Settings
                //    )
                //)
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.OldList,
                    L("Category"),
                    icon: "icon-list",
                    requiredPermissionName: AppPermissions.Pages_List
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OldListCause,
                        L("CauseOfLoss"),
                        url: "tenant.cause",
                        requiredPermissionName: AppPermissions.Pages_List_AdminCauseOfloss
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OldListTypeOfLoss,
                        L("TypeOfLoss"),
                        url: "tenant.typeOfLoss",
                        requiredPermissionName: AppPermissions.Pages_List_AdminTypeOfLoss
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OldListBorderauxStatus,
                        L("BorderauxStatus"),
                        url: "tenant.borderauxStatus",
                        requiredPermissionName: AppPermissions.Pages_List_AdminClaimStatuses
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OldListFollowUp,
                        L("FollowUp"),
                        url: "tenant.followUp",
                        requiredPermissionName: AppPermissions.Pages_List_AdminFollowUp
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OldListExpenseType,
                        L("TypeOfExpense"),
                        url: "tenant.expenseType",
                        requiredPermissionName: AppPermissions.Pages_List_AdminTypeOfExpense
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OldListClaimType,
                        L("TypeClaim"),
                        url: "tenant.claimType",
                        requiredPermissionName: AppPermissions.Pages_List_AdminTypeOfClaim
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OldListReport,
                        L("TypeofReport"),
                        url: "tenant.report",
                        requiredPermissionName: AppPermissions.Pages_List_AdminIssuingReport
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OldListTaskName,
                        L("JobCode"),
                        url: "tenant.taskName",
                        requiredPermissionName: AppPermissions.Pages_List_AdminJobCode
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OldListFirstRef,
                        L("FirstRef"),
                        url: "tenant.ViewFirstRef",
                        requiredPermissionName: AppPermissions.Pages_List_AdminFirstYearRefID
                        )
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Customers,
                    L("Customers"),
                    icon: "fa fa-group",
                    url: "tenant.customers",
                    requiredPermissionName: AppPermissions.Pages_ManageCustomers
                    )

                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.OldList,
                    L("ClaimsManagement"),
                    icon: "fa fa-edit",
                    requiredPermissionName: AppPermissions.Pages_ClaimsManagement
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.ClaimsManagement,
                        L("Claims"),
                        url: "tenant.claim",
                        requiredPermissionName: AppPermissions.Pages_ClaimsManagement
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.UpdateBordereaux,
                        L("UpdateBordereaux"),
                        url: "tenant.updateBorderaux",
                        requiredPermissionName: AppPermissions.Pages_ClaimsManagement_UpdateMyBorderauxStatus
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.UpdateClaimProcess,
                        L("UpdateClaimProcess"),
                        url: "tenant.updateClaimProcess",
                        requiredPermissionName: AppPermissions.Pages_ClaimsManagement_UpdateClaimProcess
                        )
                    )


                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.DeadlineManagement,
                    L("DeadlinesManagement"),
                    icon: "fa fa-calendar-check-o",
                    url: "tenant.viewDeadLine",
                    requiredPermissionName: AppPermissions.Pages_DeadlinesManagement
                      )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.TimeSheetsManagement,
                    L("TimesheetsManagement"),
                    icon: "fa fa-calendar",
                        url: "tenant.viewTimeSheet",
                        requiredPermissionName: AppPermissions.Pages_TimesheetsManagement
                    )

                    ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.FinanceAccountting,
                    L("FinanceAccounting"),
                    icon: "fa fa-line-chart",
                    requiredPermissionName: AppPermissions.Pages_FinanceAccounting
                        ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.ManageInvoice,
                        L("ManageInvoice"),
                        url: "tenant.viewInvoiceManage",
                        requiredPermissionName: AppPermissions.Pages_FinanceAccounting_ManageInvoice
                        )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.AccountsReceivable,
                          L("AccountsReceivable"),
                          url: "tenant.viewAccountsReceivable",
                          requiredPermissionName: AppPermissions.Pages_FinanceAccounting_ManageClaimReceiptment
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.Revenue,
                          L("Revenue"),
                          url: "tenant.ViewRevenue",
                          requiredPermissionName: AppPermissions.Pages_FinanceAccounting_Revenue
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.VAT,
                          L("VAT"),
                          url: "tenant.viewVAT",
                          requiredPermissionName: AppPermissions.Pages_FinanceAccounting_VAT
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.OtherReceiptsPayments,
                          L("OtherReceiptsPayments"),
                          url: "tenant.viewOtherReceiptsPayments",
                          requiredPermissionName: AppPermissions.Pages_FinanceAccounting_ManageCash
                          )
                      )

                    ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.CRS,
                    L("CRS"),
                    icon: "icon-list",
                    requiredPermissionName: AppPermissions.Pages_CRS
                     ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Contribution,
                        L("Contribution"),
                        url: "tenant.viewUserContribution",
                        requiredPermissionName: AppPermissions.Pages_CRS_UserContribution
                        )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.OtherWorks,
                          L("OtheWorks"),
                          url: "tenant.viewOtherWorks",
                          requiredPermissionName: AppPermissions.Pages_CRS_ContributionAdjustment
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.Income,
                          L("Income"),
                          url: "tenant.viewIncome",
                          requiredPermissionName: AppPermissions.Pages_CRS_EmployeeIncomeManagement
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.CRSs,
                          L("CRS"),
                          url: "tenant.viewCRS",
                          requiredPermissionName: AppPermissions.Pages_CRS_ManageContribution
                          )
                      )
                    ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.ReportExport,
                    L("ReportsExport"),
                    icon: "fa fa-clipboard",
                    requiredPermissionName: AppPermissions.Pages_ReportsExport
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.ClaimBordereaux,
                          L("ClaimBordereaux"),
                          url: "tenant.viewUserClaimBordereauxReport",
                          requiredPermissionName: AppPermissions.Pages_ReportsExport_ReportBorderaux
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.Fee,
                          L("Fee"),
                          url: "tenant.viewFeeReport",
                          requiredPermissionName: AppPermissions.Pages_ReportsExport_ReportLossAdjusterFee
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.Expenses,
                          L("Expenses"),
                          url: "tenant.viewExpensesReport",
                          requiredPermissionName: AppPermissions.Pages_ReportsExport_ReportExpense
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.MonthlyExpenses,
                          L("MonthlyExpenses"),
                          url: "tenant.viewMonthlyExpensesReport",
                          requiredPermissionName: AppPermissions.Pages_ReportsExport_MonthlyExpenses
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.ActualFee,
                          L("ActualFee"),
                          url: "tenant.viewActualFeeReport",
                          requiredPermissionName: AppPermissions.Pages_ReportsExport_ActualFee
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.WIP,
                          L("WIP"),
                          url: "tenant.viewWIPReport",
                          requiredPermissionName: AppPermissions.Pages_ReportsExport_WIPReport
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.RevenueR,
                          L("Revenue"),
                          url: "tenant.viewRevenueReport",
                          requiredPermissionName: AppPermissions.Pages_ReportsExport_ReportRevenue
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.AccountsReceivableR,
                          L("AccountsReceivable"),
                          url: "tenant.viewAccountsReceivableReport",
                          requiredPermissionName: AppPermissions.Pages_ReportsExport_AccountsReceivable
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.CashonHand,
                          L("CashonHand"),
                          url: "tenant.viewCashOnHandReport",
                          requiredPermissionName: AppPermissions.Pages_ReportsExport_ReportCash
                          )
                          ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.CashinBank,
                          L("CashinBank"),
                          url: "tenant.viewCashInBankReport",
                          requiredPermissionName: AppPermissions.Pages_ReportsExport_ReportBank
                          )
                          
                      )
                    ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Library,
                    L("Library"),
                    url: "tenant.ViewLibraryUpload",
                    icon: "fa fa-archive",
                    requiredPermissionName: AppPermissions.Pages_Lirary
                          )/*.AddItem(new MenuItemDefinition(
                          PageNames.App.Common.LibraryUpload,
                          L("Upload"),
                          url: "tenant.ViewLibraryUpload"                          
                          )
                           ).AddItem(new MenuItemDefinition(
                          PageNames.App.Common.LibraryDownload,
                          L("Download"),
                          url: "tenant.ViewLibraryDownload"
                          )

                          )*/
                      );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, tlsConsts.LocalizationSourceName);
        }
    }
}
