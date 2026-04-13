namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCoOwnerClaim : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.CoOwnerClaim");
            //AlterColumn("dbo.CoOwnerClaim", "Id", c => c.Int(nullable: false));
            //AlterColumn("dbo.CoOwnerClaim", "ID", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.CoOwnerClaim", "ID");
        }
        
        public override void Down()
        {
            //DropPrimaryKey("dbo.CoOwnerClaim");
            //AlterColumn("dbo.CoOwnerClaim", "ID", c => c.Int(nullable: false));
            //AlterColumn("dbo.CoOwnerClaim", "Id", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.CoOwnerClaim", "ID");
        }
    }
}
