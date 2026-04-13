namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateContributionAdjustmentRemoveColumnEmployee1 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.ContributionAdjustment", "Employee1_Id", "dbo.Employee");
            //DropIndex("dbo.ContributionAdjustment", new[] { "Employee1_Id" });
            //DropColumn("dbo.ContributionAdjustment", "Employee1_Id");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.ContributionAdjustment", "Employee1_Id", c => c.Int());
            //CreateIndex("dbo.ContributionAdjustment", "Employee1_Id");
            //AddForeignKey("dbo.ContributionAdjustment", "Employee1_Id", "dbo.Employee", "Id");
        }
    }
}
