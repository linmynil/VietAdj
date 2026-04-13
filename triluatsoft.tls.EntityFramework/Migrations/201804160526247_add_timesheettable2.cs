namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_timesheettable2 : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.TimeSheet", "TimeSheetID");
            //RenameColumn(table: "dbo.TimeSheet", name: "Id", newName: "TimeSheetID");
            //DropPrimaryKey("dbo.TimeSheet");
            //AlterColumn("dbo.TimeSheet", "TimeSheetID", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.TimeSheet", "TimeSheetID");
        }
        
        public override void Down()
        {
            //DropPrimaryKey("dbo.TimeSheet");
            //AlterColumn("dbo.TimeSheet", "TimeSheetID", c => c.Int(nullable: false));
            //AddPrimaryKey("dbo.TimeSheet", "TimeSheetID");
            //RenameColumn(table: "dbo.TimeSheet", name: "TimeSheetID", newName: "Id");
            //AddColumn("dbo.TimeSheet", "TimeSheetID", c => c.Int(nullable: false, identity: true));
        }
    }
}
