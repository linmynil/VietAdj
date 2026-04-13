namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtaskentity : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Task",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Type = c.Boolean(),
            //            TaskNameID = c.Int(),
            //            Description = c.String(),
            //            StartDate = c.DateTime(),
            //            EndDate = c.DateTime(),
            //            ClaimID = c.String(maxLength: 128),
            //            EmployeeID = c.Int(),
            //            CreatedBy = c.Int(),
            //            IsCompleted = c.Boolean(),
            //            CreatedDate = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Claim", t => t.ClaimID)
            //    .ForeignKey("dbo.Employee", t => t.EmployeeID)
            //    .ForeignKey("dbo.TaskName", t => t.TaskNameID)
            //    .Index(t => t.TaskNameID)
            //    .Index(t => t.ClaimID)
            //    .Index(t => t.EmployeeID);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Task", "TaskNameID", "dbo.TaskName");
            //DropForeignKey("dbo.Task", "EmployeeID", "dbo.Employee");
            //DropForeignKey("dbo.Task", "ClaimID", "dbo.Claim");
            //DropIndex("dbo.Task", new[] { "EmployeeID" });
            //DropIndex("dbo.Task", new[] { "ClaimID" });
            //DropIndex("dbo.Task", new[] { "TaskNameID" });
            //DropTable("dbo.Task");
        }
    }
}
