using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace triluatsoft.tls.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));
            //VietAdjuster

            //Customer
            var customer = pages.CreateChildPermission(AppPermissions.Pages_ManageCustomers, L("ManageCustomer"));
            //End
            //Dashboard
            //var dashboard = pages.CreateChildPermission(AppPermissions.Pages_Dashboard, L("Dashboard"));
            //dashboard.CreateChildPermission(AppPermissions.Pages_Dashboard_ViewMyDashboardClaim, L("ViewMyDashboardClaim"));
            //dashboard.CreateChildPermission(AppPermissions.Pages_Dashboard_ViewMyDashboardDeadline, L("ViewMyDashboardDeadline"));
            //dashboard.CreateChildPermission(AppPermissions.Pages_Dashboard_ViewMyDashboardNotification, L("ViewMyDashboardNotification"));
            //dashboard.CreateChildPermission(AppPermissions.Pages_Dashboard_ViewSystemDashboardClaim, L("ViewSystemDashboardClaim"));
            //dashboard.CreateChildPermission(AppPermissions.Pages_Dashboard_ViewSystemDashboardNotification, L("ViewSystemDashboardNotification"));
            //End

            //List
            var list = pages.CreateChildPermission(AppPermissions.Pages_List, L("Category"));
            list.CreateChildPermission(AppPermissions.Pages_List_ManageEmployee, L("ManageEmployee"));
            list.CreateChildPermission(AppPermissions.Pages_List_AdminCauseOfloss, L("CauseOfLoss"));
            list.CreateChildPermission(AppPermissions.Pages_List_AdminTypeOfLoss, L("AdminTypeOfLoss"));
            list.CreateChildPermission(AppPermissions.Pages_List_AdminClaimStatuses, L("AdminClaimStatuses"));
            list.CreateChildPermission(AppPermissions.Pages_List_AdminFollowUp, L("AdminFollowUp"));
            list.CreateChildPermission(AppPermissions.Pages_List_AdminTypeOfExpense, L("AdminTypeOfExpense"));
            list.CreateChildPermission(AppPermissions.Pages_List_AdminIssuingReport, L("AdminIssuingReport"));
            list.CreateChildPermission(AppPermissions.Pages_List_AdminJobCode, L("AdminJobCode"));
            list.CreateChildPermission(AppPermissions.Pages_List_AdminTypeOfClaim, L("AdminTypeOfClaim"));
            list.CreateChildPermission(AppPermissions.Pages_List_AdminFirstYearRefID, L("AdminFirstYearRefID"));
            //End

            //Library
            var library = pages.CreateChildPermission(AppPermissions.Pages_Lirary, L("Library"));
            library.CreateChildPermission(AppPermissions.Pages_Lirary_AdminClaimFolderPermission, L("AdminClaimFolderPermission"));
            library.CreateChildPermission(AppPermissions.Pages_Lirary_ViewEDocs, L("ViewEDocs"));
            library.CreateChildPermission(AppPermissions.Pages_Lirary_ViewClaimFolder, L("ViewClaimFolder"));
            library.CreateChildPermission(AppPermissions.Pages_Lirary_ViewDetailClaimFolder, L("ViewDetailClaimFolder"));
            library.CreateChildPermission(AppPermissions.Pages_Lirary_AdminUploadEDocs, L("AdminUploadEDocs"));
            //End

            //Claims
            var claims = pages.CreateChildPermission(AppPermissions.Pages_ClaimsManagement, L("ClaimsManagement"));
            claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_SearchClaim, L("SearchClaim"));
            claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_CreateClaim, L("CreateClaim"));
            claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_ViewAllClaim, L("ViewAllClaim"));
            claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_ViewMyClaim, L("ViewMyClaim"));
            //claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_EditSystemClaim, L("EditSystemClaim"));
            claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_EditMyClaims, L("EditMyClaim"));
            claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_AssignEmployeeToClaim, L("AssignEmployeeToClaim"));
            //claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_UpdateSystemClaimStatus, L("UpdateSystemClaimStatus"));
            claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_UpdateMyClaimStatus, L("UpdateMyClaimStatus"));
            claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_UpdateAllBorderauxStatus, L("UpdateAllBorderauxStatus"));
            claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_UpdateMyBorderauxStatus, L("UpdateMyBorderauxStatus"));
            claims.CreateChildPermission(AppPermissions.Pages_ClaimsManagement_UpdateClaimProcess, L("UpdateClaimProcess"));

            //Deadline
            var deadline = pages.CreateChildPermission(AppPermissions.Pages_DeadlinesManagement, L("DeadlinesManagement"));
            deadline.CreateChildPermission(AppPermissions.Pages_DeadlinesManagement_SearchDeadline, L("SearchDeadline"));
            //deadline.CreateChildPermission(AppPermissions.Pages_DeadlinesManagement_ViewSystemDeadline, L("ViewSystemDeadline"));
            deadline.CreateChildPermission(AppPermissions.Pages_DeadlinesManagement_CreateDeadline, L("CreateDeadline"));
            //deadline.CreateChildPermission(AppPermissions.Pages_DeadlinesManagement_EditSystemDeadline, L("EditSystemDeadline"));
            deadline.CreateChildPermission(AppPermissions.Pages_DeadlinesManagement_EditMyDeadline, L("EditMyDeadline"));
            //deadline.CreateChildPermission(AppPermissions.Pages_DeadlinesManagement_DeleteSystemDeadline, L("DeleteSystemDeadline"));
            deadline.CreateChildPermission(AppPermissions.Pages_DeadlinesManagement_DeleteMyDeadline, L("DeleteMyDeadline"));

            //Timesheets
            var timesheets = pages.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement, L("TimesheetsManagement"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_CreateTimesheet, L("CreateTimesheet"));            
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_ManageMyTimesheet, L("ManageMyTimesheet"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_ManageAMTimesheet, L("AMTimesheetManagement"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_ManageAllTimesheet, L("ManageAllTimesheet"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_ManageIssuedTimesheetOnly, L("ManageIssuedTimesheetOnly"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_ManageNotIssuedTimesheetOnly, L("ManageNotIssuedTimesheetOnly"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_EditTimesheet, L("MnEditTimesheet"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_EditAMTimesheet, L("EditAMTimesheet"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_EditMyTimesheet, L("EditMyTimesheet"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_EnterMyProfFeeExpense, L("EnterMyProfFeeExpense"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_ViewAllProFee, L("ViewAllProFee"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_ViewAllProFeeAM, L("ViewAllProFeeAM"));
            //timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_IssueSystemTimesheet, L("IssueSystemTimesheet"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_IssueMyTimesheet, L("IssueMyTimesheet"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_TransferTimesheet, L("TransferTimeSheet"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_SubmitTimesheet, L("SubmitTimesheet"));
            timesheets.CreateChildPermission(AppPermissions.Pages_TimesheetsManagement_DeleteTimesheet, L("DeleteTimeSheet"));
            //End

            //FinanceAccounting
            var finance = pages.CreateChildPermission(AppPermissions.Pages_FinanceAccounting, L("FinanceAccounting"));
            finance.CreateChildPermission(AppPermissions.Pages_FinanceAccounting_CreateInvoice, L("CreateInvoice"));
            finance.CreateChildPermission(AppPermissions.Pages_FinanceAccounting_ManageInvoice, L("ManageInvoice"));
            finance.CreateChildPermission(AppPermissions.Pages_FinanceAccounting_Revenue, L("Revenue"));
            finance.CreateChildPermission(AppPermissions.Pages_FinanceAccounting_VAT, L("VAT"));
            finance.CreateChildPermission(AppPermissions.Pages_FinanceAccounting_ManageClaimReceiptment, L("ManageClaimReceiptment"));
            finance.CreateChildPermission(AppPermissions.Pages_FinanceAccounting_ManageAR, L("ManageAR"));
            finance.CreateChildPermission(AppPermissions.Pages_FinanceAccounting_ManageCash, L("ManageCash"));
            finance.CreateChildPermission(AppPermissions.Pages_FinanceAccounting_ContributionManagement, L("ContributionManagement"));
            //finance.CreateChildPermission(AppPermissions.Pages_FinanceAccounting_ManageSystemTimesheet, L("ManageSystemTimesheet"));

            //CRS
            var crs = pages.CreateChildPermission(AppPermissions.Pages_CRS, L("CRS"));
            crs.CreateChildPermission(AppPermissions.Pages_CRS_UserContribution, L("UserContribution"));
            crs.CreateChildPermission(AppPermissions.Pages_CRS_ContributionAdjustment, L("ContributionAdjustment"));
            crs.CreateChildPermission(AppPermissions.Pages_CRS_EmployeeIncomeManagement, L("EmployeeIncomeManagement"));
            crs.CreateChildPermission(AppPermissions.Pages_CRS_ManageContribution, L("ManageContribution"));
            //End
            
            //report
            var report = pages.CreateChildPermission(AppPermissions.Pages_ReportsExport, L("ReportsExport"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ReportDebitNote, L("ReportDebitNote"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ReportExpense, L("ReportExpense"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_MonthlyExpenses, L("ReportMonthlyExpense"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ActualFee, L("ReportActualFee"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ActualFeeAll, L("ReportActualFeeAll"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_WIPReport, L("ReportWIP"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ReportBank, L("ReportBank"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ReportCash, L("ReportCash"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ReportRevenue, L("ReportRevenue"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_AccountsReceivable, L("ReportAccountsReceivable"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ReportLossAdjusterFee, L("ReportLossAdjusterFee"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ReportBorderaux, L("ReportBorderaux"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ReportAR, L("ReportAR"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ReportCashFlow, L("ReportCashFlow"));
            report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ReportDeadline, L("ReportDeadline"));
            //report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ManageSystemTimesheet, L("ManageSystemTimesheet"));
            //report.CreateChildPermission(AppPermissions.Pages_ReportsExport_ManageMyTimesheet, L("ManageMyTimesheet"));
            //End
           

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));

            //TENANT-SPECIFIC PERMISSIONS

            var dashboard =  pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);
            dashboard.CreateChildPermission(AppPermissions.Pages_Dashboard_ViewMyDashboardClaim, L("ViewMyDashboardClaim"), multiTenancySides: MultiTenancySides.Tenant);
            dashboard.CreateChildPermission(AppPermissions.Pages_Dashboard_ViewMyDashboardDeadline, L("ViewMyDashboardDeadline"), multiTenancySides: MultiTenancySides.Tenant);
            dashboard.CreateChildPermission(AppPermissions.Pages_Dashboard_ViewMyDashboardNotification, L("ViewMyDashboardNotification"), multiTenancySides: MultiTenancySides.Tenant);
            //dashboard.CreateChildPermission(AppPermissions.Pages_Dashboard_ViewSystemDashboardClaim, L("ViewSystemDashboardClaim"), multiTenancySides: MultiTenancySides.Tenant);
            //dashboard.CreateChildPermission(AppPermissions.Pages_Dashboard_ViewSystemDashboardNotification, L("ViewSystemDashboardNotification"), multiTenancySides: MultiTenancySides.Tenant);

            //administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            //administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"));

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);            

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, tlsConsts.LocalizationSourceName);
        }
    }
}
