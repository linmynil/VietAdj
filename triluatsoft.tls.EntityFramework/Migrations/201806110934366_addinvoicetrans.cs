namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinvoicetrans : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.ACT_Transaction",
            //    c => new
            //        {
            //            TransactionID = c.Int(nullable: false, identity: true),
            //            CustomerID = c.Int(),
            //            InvoiceID = c.Int(),
            //            LiabilitiesID = c.Int(),
            //            PaymentCode = c.String(),
            //            RefCode = c.String(),
            //            PaymentType = c.String(),
            //            PaymentMethod = c.String(),
            //            PaymentAMT = c.Decimal(precision: 18, scale: 2),
            //            PaymentDate = c.DateTime(),
            //            TransType = c.String(),
            //            Remark = c.String(),
            //            CurrentBalance = c.Decimal(precision: 18, scale: 2),
            //            TranID = c.Int(),
            //            CreateDate = c.DateTime(),
            //            CreateBy = c.Int(),
            //            IsDelete = c.Boolean(),
            //            IsNonInvoice = c.Boolean(),
            //            CurrentBalanceCredit = c.Decimal(precision: 18, scale: 2),
            //            CurrentBalanceDebit = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.TransactionID);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.ACT_Transaction");
        }
    }
}
