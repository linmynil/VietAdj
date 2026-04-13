using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using triluatsoft.tls.Authorization.Roles;
using triluatsoft.tls.Authorization.Users;
using triluatsoft.tls.Chat;
using triluatsoft.tls.Friendships;
using triluatsoft.tls.MultiTenancy;
using triluatsoft.tls.Storage;
using triluatsoft.tls.OldTable;

namespace triluatsoft.tls.EntityFramework
{
    /* Constructors of this DbContext is important and each one has it's own use case.
     * - Default constructor is used by EF tooling on design time.
     * - constructor(nameOrConnectionString) is used by ABP on runtime.
     * - constructor(existingConnection) is used by unit tests.
     * - constructor(existingConnection,contextOwnsConnection) can be used by ABP if DbContextEfTransactionStrategy is used.
     * See http://www.aspnetboilerplate.com/Pages/Documents/EntityFramework-Integration for more.
     */

    public class tlsDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        /* Define an IDbSet for each entity of the application */

        public virtual IDbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual IDbSet<Friendship> Friendships { get; set; }

        public virtual IDbSet<ChatMessage> ChatMessages { get; set; }
        public virtual IDbSet<Claim> Claims { get; set; }
        public virtual IDbSet<Cause> Causes { get; set; }
        public virtual IDbSet<TimeSheet> TimeSheets { get; set; }
        public virtual IDbSet<Invoice> Invoices { get; set; }
        public virtual IDbSet<CoOwnerClaim> CoOwnerClaims { get; set; }
        public virtual IDbSet<EmployeeClaim> EmployeeClaims { get; set; }
        public virtual IDbSet<Bordereaux> Bordereauxs { get; set; }
        public virtual IDbSet<BordereauxStatus> BordereauxStatuses { get; set; }
        public virtual IDbSet<RefStatus> RefStatuses { get; set; }
        public virtual IDbSet<Report> Reports { get; set; }
        public virtual IDbSet<FollowUp> FollowUps { get; set; }
        public virtual IDbSet<Customer> Customers { get; set; }
        public virtual IDbSet<Employee> Employees { get; set; }
        public virtual IDbSet<Expense> Expenses { get; set; }
        public virtual IDbSet<ExpenseType> ExpenseTypes { get; set; }
        public virtual IDbSet<ProfessionalFee> ProfessionalFees { get; set; }
        public virtual IDbSet<TaskName> TaskNames { get; set; }
        public virtual IDbSet<Task> Tasks { get; set; }
        public virtual IDbSet<ACT_Liabilities> ACT_Liabilities { get; set; }
        public virtual IDbSet<ACT_Revenue> ACT_Revenues { get; set; }
        public virtual IDbSet<Invoice_Timesheet> Invoice_Timesheets { get; set; }
        public virtual IDbSet<ACT_Transaction> ACT_Transactions { get; set; }
        public virtual IDbSet<ACT_CommissionPayment> ACT_CommissionPayments { get; set; }
        public virtual IDbSet<CommissionPolicy> CommissionPolicies { get; set; }
        public virtual IDbSet<CustomerOfficer> CustomerOfficers { get; set; }
        public virtual IDbSet<Commission> Commissions { get; set; }
        public virtual IDbSet<Cash> Cashs { get; set; }
        public virtual IDbSet<CashHistory> CashHistorys { get; set; }
        public virtual IDbSet<ContributionAdjustment> ContributionAdjustments { get; set; }
        public virtual IDbSet<F> Fs { get; set; }
        public virtual IDbSet<EmployeeIncome> EmployeeIncomes { get; set; }
        public virtual IDbSet<CIncomeType> CIncomeTypes { get; set; }
        public virtual IDbSet<FileUpload> FileUpLoad { get; set; }
        public virtual IDbSet<ClaimProcess> ClaimProcess { get; set; }
        public tlsDbContext()
            : base("Default")
        {
            
        }

        public tlsDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public tlsDbContext(DbConnection existingConnection)
           : base(existingConnection, false)
        {

        }

        public tlsDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {

        }
        
    }
}
