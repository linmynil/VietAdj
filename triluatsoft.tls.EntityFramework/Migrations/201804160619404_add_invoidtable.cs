namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_invoidtable : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Invoice",
            //    c => new
            //        {
            //            InvoiceID = c.Int(nullable: false, identity: true),
            //            InvoiceID1 = c.Int(nullable: false),
            //            CustomerID = c.Int(),
            //            ClaimID = c.String(maxLength: 128),
            //            InvoiceCode = c.String(),
            //            InvoiceDate = c.DateTime(),
            //            IsAdvInvoice = c.Boolean(),
            //            ProFeeGrandAMT = c.Decimal(precision: 18, scale: 2),
            //            ExpenseAMT = c.Decimal(precision: 18, scale: 2),
            //            RevenueAMT = c.Decimal(precision: 18, scale: 2),
            //            TaxAMT = c.Decimal(precision: 18, scale: 2),
            //            TotalAMT = c.Decimal(precision: 18, scale: 2),
            //            AdvRemainAMT = c.Decimal(precision: 18, scale: 2),
            //            Remark = c.String(),
            //            CreateDate = c.DateTime(),
            //            CreateBy = c.Int(),
            //            UpdateDate = c.DateTime(),
            //            UpdateBy = c.Int(),
            //            IsSent2Customer = c.Boolean(),
            //            SentDate = c.DateTime(),
            //            SentBy = c.Int(),
            //            IsDeleted = c.Boolean(),
            //            IsSettled = c.Boolean(),
            //            SettlementDate = c.DateTime(),
            //            CommisionAMT = c.Decimal(precision: 18, scale: 2),
            //            ProFeeAMT = c.Decimal(precision: 18, scale: 2),
            //            DiscountAMT = c.Decimal(precision: 18, scale: 2),
            //            AdvanceDate = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.InvoiceID)
            //    .ForeignKey("dbo.Claim", t => t.ClaimID)
            //    .Index(t => t.ClaimID);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Invoice", "ClaimID", "dbo.Claim");
            //DropIndex("dbo.Invoice", new[] { "ClaimID" });
            //DropTable("dbo.Invoice");
        }
    }
}
