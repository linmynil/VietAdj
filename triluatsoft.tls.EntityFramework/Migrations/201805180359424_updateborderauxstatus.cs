namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateborderauxstatus : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Bordereaux", "BordereauxStatusID", c => c.String(maxLength: 128));
            //CreateIndex("dbo.Bordereaux", "BordereauxStatusID");
            //AddForeignKey("dbo.Bordereaux", "BordereauxStatusID", "dbo.BordereauxStatus", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Bordereaux", "BordereauxStatusID", "dbo.BordereauxStatus");
            //DropIndex("dbo.Bordereaux", new[] { "BordereauxStatusID" });
            //AlterColumn("dbo.Bordereaux", "BordereauxStatusID", c => c.String());
        }
    }
}
