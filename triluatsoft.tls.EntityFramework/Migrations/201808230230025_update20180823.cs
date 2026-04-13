namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update20180823 : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.tblFolder", "ID", c => c.Int(nullable: false, identity: true));
            //CreateTable(
            //    "dbo.tblFolder",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            FName = c.String(),
            //            FPath = c.String(),
            //            FSecurity = c.String(),
            //            isUpload = c.Boolean(),
            //            CreatedDate = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);

            //AddColumn("dbo.Claim", "tblFolder_Id", c => c.Int());
            //CreateIndex("dbo.Claim", "tblFolder_Id");
            //AddForeignKey("dbo.Claim", "tblFolder_Id", "dbo.tblFolder", "Id");
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.tblFolder", "Id", c => c.Int(nullable: false, identity: true));
            //DropForeignKey("dbo.Claim", "tblFolder_Id", "dbo.tblFolder");
            //DropIndex("dbo.Claim", new[] { "tblFolder_Id" });
            //DropColumn("dbo.Claim", "tblFolder_Id");
            //DropTable("dbo.tblFolder");
        }
    }
}
