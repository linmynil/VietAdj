namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateinvoice : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.ACT_Liabilities",
            //    c => new
            //        {
            //            LiabilitiesID = c.Int(nullable: false, identity: true),
            //            ClaimID = c.String(),
            //            InvoiceID = c.Int(),
            //            CustomerID = c.Int(),
            //            ProFeeGrandAMT = c.Decimal(precision: 18, scale: 2),
            //            ExpenseAMT = c.Decimal(precision: 18, scale: 2),
            //            TaxAMT = c.Decimal(precision: 18, scale: 2),
            //            TotalAMT = c.Decimal(precision: 18, scale: 2),
            //            RemainAMT = c.Decimal(precision: 18, scale: 2),
            //            PaidAMT = c.Decimal(precision: 18, scale: 2),
            //            CurrentBalance = c.Decimal(precision: 18, scale: 2),
            //            TranID = c.Int(),
            //            CreateDate = c.DateTime(),
            //            CreateBy = c.Int(),
            //            LastPaidDate = c.DateTime(),
            //            IsDelete = c.Boolean(),
            //            IsSettled = c.Boolean(),
            //            SettlementDate = c.DateTime(),
            //            CurrentBalanceCredit = c.Decimal(precision: 18, scale: 2),
            //            CurrentBalanceDebit = c.Decimal(precision: 18, scale: 2),
            //            RevenueAMT = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.LiabilitiesID);
            
            //CreateTable(
            //    "dbo.ACT_Revenue",
            //    c => new
            //        {
            //            RevenueID = c.Int(nullable: false, identity: true),
            //            ClaimID = c.String(),
            //            InvoiceID = c.Int(),
            //            CustomerID = c.Int(),
            //            RevenueAMT = c.Decimal(precision: 18, scale: 2),
            //            CreateDate = c.DateTime(),
            //            CreateBy = c.Int(),
            //            IsDelete = c.Boolean(),
            //        })
            //    .PrimaryKey(t => t.RevenueID);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.ACT_Revenue");
            //DropTable("dbo.ACT_Liabilities");
        }
    }
}
