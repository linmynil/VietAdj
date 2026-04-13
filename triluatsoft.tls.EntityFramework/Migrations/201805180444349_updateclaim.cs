namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateclaim : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Claim", "Bordereaux_Id", c => c.Int());
            //AlterColumn("dbo.Bordereaux", "ClaimID", c => c.String(maxLength: 128));
            //CreateIndex("dbo.Bordereaux", "ClaimID");
            //CreateIndex("dbo.Claim", "Bordereaux_Id");
            //AddForeignKey("dbo.Bordereaux", "ClaimID", "dbo.Claim", "Id");
            //AddForeignKey("dbo.Claim", "Bordereaux_Id", "dbo.Bordereaux", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Claim", "Bordereaux_Id", "dbo.Bordereaux");
            //DropForeignKey("dbo.Bordereaux", "ClaimID", "dbo.Claim");
            //DropIndex("dbo.Claim", new[] { "Bordereaux_Id" });
            //DropIndex("dbo.Bordereaux", new[] { "ClaimID" });
            //AlterColumn("dbo.Bordereaux", "ClaimID", c => c.String());
            //DropColumn("dbo.Claim", "Bordereaux_Id");
        }
    }
}
