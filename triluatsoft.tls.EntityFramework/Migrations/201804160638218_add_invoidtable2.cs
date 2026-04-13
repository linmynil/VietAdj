namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_invoidtable2 : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Invoice", "InvoiceID1");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Invoice", "InvoiceID1", c => c.Int(nullable: false));
        }
    }
}
