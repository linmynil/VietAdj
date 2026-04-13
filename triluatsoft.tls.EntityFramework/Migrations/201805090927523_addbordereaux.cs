namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addbordereaux : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Bordereaux",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ClaimID = c.String(),
            //            DateOfSurvey = c.DateTime(),
            //            ReportID = c.Int(),
            //            DateOfReport = c.DateTime(),
            //            BordereauxStatusID = c.String(),
            //            OtherStatus = c.String(),
            //            FollowUpID = c.Int(),
            //            OtherFollowUp = c.String(),
            //            CreatedDate = c.DateTime(),
            //            CreatedBy = c.Int(),
            //            Reserve = c.Decimal(precision: 18, scale: 2),
            //            ProgressPayment = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.BordereauxStatus",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(),
            //            isActive = c.Boolean(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.BordereauxStatus");
            //DropTable("dbo.Bordereaux");
        }
    }
}
