namespace triluatsoft.tls.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";
        
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";
        
        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";

        //Customer
        public const string Pages_ManageCustomers = "Pages.ManageCustomers";
        //END
        

        //Dashboard
        public const string Pages_Dashboard = "Pages.Dashboard";
        public const string Pages_Dashboard_ViewSystemDashboardClaim = "Pages.Dashboard.ViewSystemDashboardClaim";
        public const string Pages_Dashboard_ViewMyDashboardClaim = "Pages.Dashboard.ViewMyDashboardClaim";
        public const string Pages_Dashboard_ViewMyDashboardDeadline = "Pages.Dashboard.ViewMyDashboardDeadline";
        public const string Pages_Dashboard_ViewMyDashboardNotification = "Pages.Dashboard.ViewMyDashboardNotification";
        public const string Pages_Dashboard_ViewSystemDashboardNotification = "Pages.Dashboard.ViewSystemDashboardNotification";

        //List
        public const string Pages_List = "Pages.List";
        public const string Pages_List_ManageEmployee = "Pages.List.ManageEmployee";
        public const string Pages_List_AdminCauseOfloss = "Pages.List.AdminCauseOfloss";
        public const string Pages_List_AdminTypeOfLoss = "Pages.List.AdminTypeOfLoss";
        public const string Pages_List_AdminClaimStatuses = "pages.List.AdminClaimStatuses";
        public const string Pages_List_AdminFollowUp = "Pages.List.AdminFollowUp";
        public const string Pages_List_AdminTypeOfExpense = "Pages.List.AdminTypeOfExpense";
        public const string Pages_List_AdminIssuingReport = "Pages.List.AdminIssuingReport";
        public const string Pages_List_AdminJobCode = "Pages.List.AdminJobCode";
        public const string Pages_List_AdminTypeOfClaim = "Pages.List.AdminTypeOfClaim";
        public const string Pages_List_AdminFirstYearRefID = "Pages.List.AdminFirstYearRefID";
        //END

        //Library
        public const string Pages_Lirary = "Pages.Lirary";
        public const string Pages_Lirary_AdminClaimFolderPermission = "Pages.Lirary.AdminClaimFolderPermission";
        public const string Pages_Lirary_ViewEDocs = "Pages.Lirary.ViewEDocs";
        public const string Pages_Lirary_ViewClaimFolder = "Pages.Lirary.ViewClaimFolder";
        public const string Pages_Lirary_ViewDetailClaimFolder = "Pages.Lirary.ViewDetailClaimFolder";
        public const string Pages_Lirary_AdminUploadEDocs = "Pages.Lirary.AdminUploadEDocs";
        

        //ClaimsManagement
        public const string Pages_ClaimsManagement = "Pages.ClaimsManagement"; 
        public const string Pages_ClaimsManagement_SearchClaim = "Pages.ClaimsManagement.SearchClaim";
        public const string Pages_ClaimsManagement_CreateClaim = "Pages.ClaimsManagement.CreateClaim";
        public const string Pages_ClaimsManagement_ViewAllClaim = "Pages.ClaimsManagement.ViewAllClaim";
        public const string Pages_ClaimsManagement_ViewMyClaim = "Pages.ClaimsManagement.ViewMyClaim";
        public const string Pages_ClaimsManagement_EditSystemClaim = "Pages.ClaimsManagement.EditSystemClaim";
        public const string Pages_ClaimsManagement_EditMyClaims = "pages.ClaimsManagement.EditMyClaims";
        public const string Pages_ClaimsManagement_AssignEmployeeToClaim = "Pages.ClaimsManagement.AssignEmployeeToClaim";
        public const string Pages_ClaimsManagement_UpdateSystemClaimStatus = "Pages.ClaimsManagement.UpdateSystemClaimStatus";
        public const string Pages_ClaimsManagement_UpdateMyClaimStatus = "Pages.ClaimsManagement.UpdateMyClaimStatus";
        public const string Pages_ClaimsManagement_UpdateAllBorderauxStatus = "Pages.ClaimsManagement.UpdateAllBorderauxStatus";
        public const string Pages_ClaimsManagement_UpdateMyBorderauxStatus = "Pages.ClaimsManagement.UpdateMyBorderauxStatus";
        public const string Pages_ClaimsManagement_UpdateClaimProcess = "Pages.ClaimsManagement.UpdateClaimProcess";
        //END


        //DeadlinesManagement
        public const string Pages_DeadlinesManagement = "Pages.DeadlinesManagement";
        public const string Pages_DeadlinesManagement_SearchDeadline = "Pages.DeadlinesManagement.SearchDeadline";
        public const string Pages_DeadlinesManagement_ViewSystemDeadline = "pages.DeadlinesManagement.ViewSystemDeadline";
        public const string Pages_DeadlinesManagement_CreateDeadline = "Pages.DeadlinesManagement.CreateDeadline";
        public const string Pages_DeadlinesManagement_EditSystemDeadline = "Pages.DeadlinesManagement.EditSystemDeadline";
        public const string Pages_DeadlinesManagement_EditMyDeadline = "Pages.DeadlinesManagement.EditMyDeadline";
        public const string Pages_DeadlinesManagement_DeleteSystemDeadline = "PagesDeadlinesManagement.DeleteSystemDeadline";
        public const string Pages_DeadlinesManagement_DeleteMyDeadline = "Pages.DeadlinesManagement.DeleteMyDeadline";
        //END
        
        //TimesheetsManagement
        public const string Pages_TimesheetsManagement = "Pages.TimesheetsManagement";
        public const string Pages_TimesheetsManagement_EnterMyProfFeeExpense = "Pages.TimesheetsManagement.EnterMyProfFeeExpense";
        public const string Pages_TimesheetsManagement_IssueSystemTimesheet = "Pages.TimesheetsManagement.IssueSystemTimesheet";
        public const string Pages_TimesheetsManagement_IssueMyTimesheet = "Pages.TimesheetsManagement.IssueMyTimesheet";
        public const string Pages_TimesheetsManagement_CreateTimesheet = "Pages.TimesheetsManagement.CreateTimesheet";
        public const string Pages_TimesheetsManagement_ManageAllTimesheet = "Pages.TimesheetsManagement.ManageAllTimesheet";
        public const string Pages_TimesheetsManagement_ManageMyTimesheet = "pages.TimesheetsManagement.ManageMyTimesheet";
        public const string Pages_TimesheetsManagement_ManageAMTimesheet = "Pages.TimesheetsManagement.ManageAMTimesheet";
        public const string Pages_TimesheetsManagement_DeleteTimesheet = "Pages.TimesheetsManagement.DeleteTimesheet";
        public const string Pages_TimesheetsManagement_EditTimesheet = "Pages.TimesheetsManagement.EditTimesheet";
        public const string Pages_TimesheetsManagement_EditAMTimesheet = "Pages.TimesheetsManagement.EditAMTimesheet";
        public const string Pages_TimesheetsManagement_EditMyTimesheet = "Pages.TimesheetsManagement.EditMyTimesheet";
        public const string Pages_TimesheetsManagement_TransferTimesheet = "Pages.TimesheetsManagement.TransferTimesheet";
        public const string Pages_TimesheetsManagement_SubmitTimesheet = "Pages.TimesheetsManagement.SubmitTimesheet";
        public const string Pages_TimesheetsManagement_ManageIssuedTimesheetOnly = "Pages.TimesheetsManagement.ManageIssuedTimesheetOnly";
        public const string Pages_TimesheetsManagement_ManageNotIssuedTimesheetOnly = "Pages.TimesheetsManagement.ManageNotIssuedTimesheetOnly";
        public const string Pages_TimesheetsManagement_ViewAllProFee = "Pages.TimesheetsManagement.ViewAllProFee";
        public const string Pages_TimesheetsManagement_ViewAllProFeeAM = "Pages.TimesheetsManagement.ViewAllProFeeAM";
        //END


        //FinanceAccounting
        public const string Pages_FinanceAccounting = "Pages.FinanceAccounting";
        public const string Pages_FinanceAccounting_CreateInvoice = "Pages.FinanceAccounting.CreateInvoice";
        public const string Pages_FinanceAccounting_ManageInvoice = "pages.FinanceAccounting.ManageInvoice";
        public const string Pages_FinanceAccounting_Revenue = "Pages.FinanceAccounting.ManageRevenue";
        public const string Pages_FinanceAccounting_VAT = "pages.FinanceAccounting.ManageVAT";
        public const string Pages_FinanceAccounting_ManageClaimReceiptment = "pages.FinanceAccounting.ManageClaimReceiptment";
        public const string Pages_FinanceAccounting_ManageAR = "Pages.FinanceAccounting.ManageAR";
        public const string Pages_FinanceAccounting_ManageCash = "Pages.FinanceAccounting.ManageCash";
        //public const string Pages_FinanceAccounting_UserContribution = "Pages.FinanceAccounting.UserContribution";
        public const string Pages_FinanceAccounting_ContributionManagement = "Pages.FinanceAccounting.ContributionManagement";
        public const string Pages_FinanceAccounting_ManageSystemTimesheet = "Pages.FinanceAccounting.ManageSystemTimesheet";
        //END

        //CRS
        public const string Pages_CRS = "Pages.CRS";
        ////Contribution
        public const string Pages_CRS_UserContribution = "Pages.CRS.UserContribution";
        //Other Works
        public const string Pages_CRS_ContributionAdjustment = "Pages.CRS.ContributionAdjustment";
        //Income
        public const string Pages_CRS_EmployeeIncomeManagement = "Pages.CRS.EmployeeIncomeManagement";
        //CRS
        public const string Pages_CRS_ManageContribution = "pages.CRS.ManageContribution";
        //END


        //ReportExport
        public const string Pages_ReportsExport = "Pages.ReportsExport";
        public const string Pages_ReportsExport_ReportDebitNote = "Pages.ReportsExport.ReportDebitNote";
        public const string Pages_ReportsExport_ReportExpense = "pages.ReportsExport.ReportExpense";
        public const string Pages_ReportsExport_ReportBank = "Pages.ReportsExport.ReportBank";
        public const string Pages_ReportsExport_ReportCash = "Pages.ReportsExport.ReportCash";
        public const string Pages_ReportsExport_ReportRevenue = "pages.ReportsExport.ReportRevenue";
        public const string Pages_ReportsExport_ReportLossAdjusterFee = "Pages.ReportsExport.ReportLossAdjusterFee";
        public const string Pages_ReportsExport_ReportBorderaux = "Pages.ReportsExport.ReportBorderaux";
        public const string Pages_ReportsExport_ReportAR = "Pages.ReportsExport.ReportAR";
        public const string Pages_ReportsExport_ReportCashFlow = "Pages.ReportsExport.ReportCashFlow";
        public const string Pages_ReportsExport_ReportDeadline = "pages.ReportsExport.ReportDeadline";
        public const string Pages_ReportsExport_ManageSystemTimesheet = "Pages.ReportsExport.ManageSystemTimesheet";
        public const string Pages_ReportsExport_ManageMyTimesheet = "Pages.ReportsExport.ManageMyTimesheet";
        public const string Pages_ReportsExport_MonthlyExpenses = "Pages.ReportsExport.MonthlyExpenses";
        public const string Pages_ReportsExport_ActualFee = "Pages.ReportsExport.ActualFee";
        public const string Pages_ReportsExport_ActualFeeAll = "Pages.ReportsExport.ActualFeeAll";
        public const string Pages_ReportsExport_WIPReport = "Pages.ReportsExport.WIPReport";
        public const string Pages_ReportsExport_AccountsReceivable = "Pages.ReportsExport.AccountsReceivable";

        //END



    }
}