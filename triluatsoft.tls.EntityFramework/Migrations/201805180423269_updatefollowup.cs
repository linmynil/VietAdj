namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefollowup : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.FollowUp",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            Code = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateIndex("dbo.Bordereaux", "FollowUpID");
            //AddForeignKey("dbo.Bordereaux", "FollowUpID", "dbo.FollowUp", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Bordereaux", "FollowUpID", "dbo.FollowUp");
            //DropIndex("dbo.Bordereaux", new[] { "FollowUpID" });
            //DropTable("dbo.FollowUp");
        }
    }
}
