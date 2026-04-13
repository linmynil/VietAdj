namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update20180828 : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Claim", "FID");
            //RenameColumn(table: "dbo.Claim", name: "tblFolder_Id", newName: "FID");
            //RenameIndex(table: "dbo.Claim", name: "IX_tblFolder_Id", newName: "IX_FID");
        }
        
        public override void Down()
        {
            //RenameIndex(table: "dbo.Claim", name: "IX_FID", newName: "IX_tblFolder_Id");
            //RenameColumn(table: "dbo.Claim", name: "FID", newName: "tblFolder_Id");
            //AddColumn("dbo.Claim", "FID", c => c.Int());
        }
    }
}
