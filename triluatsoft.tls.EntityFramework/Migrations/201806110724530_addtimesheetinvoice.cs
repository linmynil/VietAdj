namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtimesheetinvoice : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Invoice_Timesheet",
            //    c => new
            //        {
            //            InvoiceDetailID = c.Int(nullable: false, identity: true),
            //            InvoiceID = c.Int(),
            //            TimeSheetID = c.Int(),
            //            CreateBy = c.Int(),
            //            CreateDate = c.DateTime(),
            //            Netfee = c.Decimal(precision: 18, scale: 2),
            //            Expene = c.Decimal(precision: 18, scale: 2),
            //            Amount = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.InvoiceDetailID);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.Invoice_Timesheet");
        }
    }
}
