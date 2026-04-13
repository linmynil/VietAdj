namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update20180827AddColumnIsDeleted_tblTimeSheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSheet", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSheet", "IsDeleted");
        }
    }
}
