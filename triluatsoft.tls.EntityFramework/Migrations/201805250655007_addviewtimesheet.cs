namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addviewtimesheet : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.VTimesheet",
            //    c => new
            //        {
            //            TID = c.Int(nullable: false),
            //            CID = c.String(maxLength: 25),
            //            IsIssued = c.Boolean(),
            //            ExchangeRate = c.Decimal(precision: 18, scale: 2),
            //            ProFeeGrandAMT = c.Decimal(precision: 18, scale: 2),
            //            ExpenseAMT = c.Decimal(precision: 18, scale: 2),
            //            TaxAMT = c.Decimal(precision: 18, scale: 2),
            //            GrandAMT = c.Decimal(precision: 18, scale: 2),
            //            IssueDate = c.DateTime(),
            //            IsSubmited = c.Boolean(),
            //            InsurerID = c.Int(),
            //            CustomerName = c.String(maxLength: 100),
            //            IsInvoiced = c.Boolean(),
            //            TimeSheetID = c.Int(),
            //            SumExpenseAMT = c.Decimal(precision: 18, scale: 2),
            //            Id = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.TID);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.VTimesheet");
        }
    }
}
