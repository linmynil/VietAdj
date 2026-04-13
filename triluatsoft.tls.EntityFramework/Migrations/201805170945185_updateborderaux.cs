namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateborderaux : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Report",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateIndex("dbo.Bordereaux", "ReportID");
            //AddForeignKey("dbo.Bordereaux", "ReportID", "dbo.Report", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Bordereaux", "ReportID", "dbo.Report");
            //DropIndex("dbo.Bordereaux", new[] { "ReportID" });
            //DropTable("dbo.Report");
        }
    }
}
