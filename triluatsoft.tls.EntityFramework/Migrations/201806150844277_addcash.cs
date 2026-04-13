namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcash : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.CashHistory",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CashID = c.Int(),
            //            InputDate = c.DateTime(),
            //            Description = c.String(),
            //            Notes = c.String(),
            //            RefNbr = c.String(),
            //            PaymentMethod = c.String(),
            //            VoucherType = c.String(),
            //            Amount = c.Decimal(precision: 18, scale: 2),
            //            CreatedDate = c.DateTime(),
            //            CashCode = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Cash", t => t.CashID)
            //    .Index(t => t.CashID);
            
            //CreateTable(
            //    "dbo.Cash",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CashCode = c.String(),
            //            RefNbr = c.String(),
            //            Description = c.String(),
            //            Notes = c.String(),
            //            PaymentMethod = c.String(),
            //            VoucherType = c.String(),
            //            Amount = c.Decimal(precision: 18, scale: 2),
            //            CreatedDate = c.DateTime(),
            //            IsDelete = c.Boolean(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.CashHistory", "CashID", "dbo.Cash");
            //DropIndex("dbo.CashHistory", new[] { "CashID" });
            //DropTable("dbo.Cash");
            //DropTable("dbo.CashHistory");
        }
    }
}
