namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updated_ClaimProcess_Add_ClaimID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimProcess", "ClaimID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClaimProcess", "ClaimID");
        }
    }
}
