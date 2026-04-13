namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateuserclaim : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.CoOwnerClaim");
            //AlterColumn("dbo.CoOwnerClaim", "Id", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.CoOwnerClaim", "Id");
        }
        
        public override void Down()
        {
            //DropPrimaryKey("dbo.CoOwnerClaim");
            //AlterColumn("dbo.CoOwnerClaim", "Id", c => c.Int(nullable: false));
            //AddPrimaryKey("dbo.CoOwnerClaim", "ID");
        }
    }
}
