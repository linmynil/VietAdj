namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateContributionAdjustment : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.ContributionAdjustment",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            EmployeeID = c.Int(nullable: false),
            //            AdjustDate = c.DateTime(),
            //            AdjustAMT = c.Decimal(precision: 18, scale: 2),
            //            CreateBy = c.Int(),
            //            CreateDate = c.DateTime(),
            //            Description = c.String(),
            //            Employee1_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Employee", t => t.EmployeeID, cascadeDelete: true)
            //    .ForeignKey("dbo.Employee", t => t.Employee1_Id)
            //    .Index(t => t.EmployeeID)
            //    .Index(t => t.Employee1_Id);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.ContributionAdjustment", "Employee1_Id", "dbo.Employee");
            //DropForeignKey("dbo.ContributionAdjustment", "EmployeeID", "dbo.Employee");
            //DropIndex("dbo.ContributionAdjustment", new[] { "Employee1_Id" });
            //DropIndex("dbo.ContributionAdjustment", new[] { "EmployeeID" });
            //DropTable("dbo.ContributionAdjustment");
        }
    }
}
