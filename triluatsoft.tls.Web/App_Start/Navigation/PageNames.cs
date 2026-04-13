namespace triluatsoft.tls.Web.Navigation
{
    public static class PageNames
    {
        public static class App
        {
            public static class Common
            {
                public const string Administration = "Administration";
                public const string Roles = "Administration.Roles";
                public const string Users = "Administration.Users";
                public const string AuditLogs = "Administration.AuditLogs";
                public const string OrganizationUnits = "Administration.OrganizationUnits";
                public const string Languages = "Administration.Languages";
                
                public const string Customers = "Customers";
                public const string CustomersInsurers = "Customers.Insurers";
                public const string CustomersBrokers = "Customers.Brokers";

                //List
                public const string OldList = "OldList";
                public const string OldListCause = "OldList.Cause";
                public const string OldListTypeOfLoss = "OldList.TypeOfLoss";
                public const string OldListBorderauxStatus = "OldList.BorderauxStatus";
                public const string OldListFollowUp = "OldList.FollowUp";
                public const string OldListExpenseType = "OldList.ExpenseType";
                public const string OldListClaimType = "OldList.ClaimType";
                public const string OldListReport = "OldList.Report";
                public const string OldListTaskName = "OldList.TaskName";
                public const string OldListFirstRef = "OldList.FirstRef";
                //claim Management
                public const string ClaimsManagement = "ClaimsManagement";
                public const string NewClaim = "ClaimsManagement.NewClaim";
                public const string EditClaim = "ClaimsManagement.EditClaim";
                public const string SearchClaim = "ClaimsManagement.SearchClaim";
                public const string ClaimStatus = "ClaimsManagement.ClaimStatus";
                public const string UpdateBordereaux = "ClaimsManagement.UpdateBordereaux";
                public const string UpdateClaimProcess = "ClaimsManagement.UpdateClaimProcess";

                //Deadline Management
                public const string DeadlineManagement = "DeadlineManagement";
                public const string SearchDeadline = "DeadlineManagement.SearchDeadline";
                public const string ManageDeadline = "DeadlineManagement.ManageDeadline";


                //TimeSheets Management
                public const string TimeSheetsManagement = "TimeSheetsManagement";
                public const string EnterTimesheet = "TimeSheetsManagement.EnterTimesheet";
                public const string ManageTimesheet = "TimeSheetsManagement.ManageTimesheet";
                public const string SubmitTimesheet = "TimeSheetsManagement.SubmitTimesheet";

                //Finance Accountting
                public const string FinanceAccountting = "FinanceAccountting";
                public const string CreateInvoice = "FinanceAccountting.CreateInvoice";
                public const string ManageInvoice = "FinanceAccountting.ManageInvoice";
                public const string AccountsReceivable = "FinanceAccountting.AccountsReceivable";
                public const string Revenue = "FinanceAccountting.Revenue";
                public const string VAT = "FinanceAccountting.VAT";
                public const string OtherReceiptsPayments = "FinanceAccountting.OtherReceiptsPayments";

                //CRS
                public const string CRS = "CRS";
                public const string Contribution = "CRS.Contribution";
                public const string OtherWorks = "CRS.OtherWorks";
                public const string Income = "CRS.Income";
                public const string CRSs = "CRS.CRS";

                //ReportExport
                public const string ReportExport = "ReportExport";
                public const string ClaimBordereaux = "ReportExport.ClaimBordereaux";
                public const string Fee = "ReportExport.Fee";
                public const string Expenses = "ReportExport.Expenses";
                public const string MonthlyExpenses = "ReportExport.Revenue";
                public const string ActualFee = "ReportExport.VAT";
                public const string WIP = "ReportExport.OtherReceiptsPayments";
                public const string RevenueR = "ReportExport.RevenueR";
                public const string AccountsReceivableR = "ReportExport.AccountsReceivableR";
                public const string CashonHand = "ReportExport.CashonHand";
                public const string CashinBank = "ReportExport.CashinBank";

                //Library
                public const string Library = "Library";
                public const string LibraryUpload = "Library.Upload";
                public const string LibraryDownload = "Library.Download";
            }

            public static class Host
            {
                public const string Tenants = "Tenants";
                public const string Editions = "Editions";
                public const string Maintenance = "Administration.Maintenance";
                public const string Settings = "Administration.Settings.Host";
            }

            public static class Tenant
            {
                public const string Dashboard = "Dashboard.Tenant";
                public const string Settings = "Administration.Settings.Tenant";
            }
        }

        public static class Frontend
        {
            public const string Home = "Frontend.Home";
            public const string About = "Frontend.About";
        }
    }
}