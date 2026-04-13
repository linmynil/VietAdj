namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DocRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimProcess", "IsDocRequest", c => c.Boolean(nullable: false));
            AddColumn("dbo.ClaimProcess", "DocRequestDeadline", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "DocRequestTime", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "DocRequestBy", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClaimProcess", "DocRequestBy");
            DropColumn("dbo.ClaimProcess", "DocRequestTime");
            DropColumn("dbo.ClaimProcess", "DocRequestDeadline");
            DropColumn("dbo.ClaimProcess", "IsDocRequest");
        }
    }
}
