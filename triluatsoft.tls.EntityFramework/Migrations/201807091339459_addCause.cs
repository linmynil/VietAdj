namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCause : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Cause",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateIndex("dbo.Claim", "CauseID");
            //AddForeignKey("dbo.Claim", "CauseID", "dbo.Cause", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Claim", "CauseID", "dbo.Cause");
            //DropIndex("dbo.Claim", new[] { "CauseID" });
            //DropTable("dbo.Cause");
        }
    }
}
