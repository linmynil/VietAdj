namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateContributionAdjustmentNullEmployee : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.ContributionAdjustment", "EmployeeID", "dbo.Employee");
            //DropIndex("dbo.ContributionAdjustment", new[] { "EmployeeID" });
            //AlterColumn("dbo.ContributionAdjustment", "EmployeeID", c => c.Int());
            //CreateIndex("dbo.ContributionAdjustment", "EmployeeID");
            //AddForeignKey("dbo.ContributionAdjustment", "EmployeeID", "dbo.Employee", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.ContributionAdjustment", "EmployeeID", "dbo.Employee");
            //DropIndex("dbo.ContributionAdjustment", new[] { "EmployeeID" });
            //AlterColumn("dbo.ContributionAdjustment", "EmployeeID", c => c.Int(nullable: false));
            //CreateIndex("dbo.ContributionAdjustment", "EmployeeID");
            //AddForeignKey("dbo.ContributionAdjustment", "EmployeeID", "dbo.Employee", "Id", cascadeDelete: true);
        }
    }
}
