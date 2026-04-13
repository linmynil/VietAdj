namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtaskname : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.TaskName",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            JobCode = c.String(),
            //            StandardTime = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.TaskName");
        }
    }
}
