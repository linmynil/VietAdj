namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtable_ACT_CommissionPayments_CommissionPolicies_CustomerOfficers_Commissions : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.ACT_CommissionPayment",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            TransactionID = c.Int(nullable: false),
            //            CommissionID = c.Int(),
            //            PaymentCode = c.String(),
            //            PaymentType = c.String(),
            //            PaymentMethod = c.String(),
            //            Remark = c.String(),
            //            PaymentAMT = c.Decimal(precision: 18, scale: 2),
            //            PaymentDate = c.DateTime(),
            //            CommissionAMT = c.Decimal(precision: 18, scale: 2),
            //            CreateDate = c.DateTime(),
            //            CreateBy = c.Int(),
            //            RemainCommissionAMT = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Commission", t => t.CommissionID)
            //    .Index(t => t.CommissionID);
            
            //CreateTable(
            //    "dbo.Commission",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CommissionID = c.Int(nullable: false),
            //            CMSPolicyID = c.Int(),
            //            OfficerID = c.Int(),
            //            InvoiceID = c.Int(),
            //            CreateBy = c.Int(),
            //            CreateDate = c.DateTime(),
            //            UpdateBy = c.Int(),
            //            UpdateDate = c.DateTime(),
            //            IsPaid = c.Boolean(),
            //            PaidDate = c.DateTime(),
            //            CommissionAMT = c.Decimal(precision: 18, scale: 2),
            //            CommissionPolicy_Id = c.Int(),
            //            CustomerOfficer_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.CommissionPolicy", t => t.CommissionPolicy_Id)
            //    .ForeignKey("dbo.CustomerOfficer", t => t.CustomerOfficer_Id)
            //    .ForeignKey("dbo.Invoice", t => t.InvoiceID)
            //    .Index(t => t.InvoiceID)
            //    .Index(t => t.CommissionPolicy_Id)
            //    .Index(t => t.CustomerOfficer_Id);
            
            //CreateTable(
            //    "dbo.CommissionPolicy",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CMSPolicyID = c.Int(nullable: false),
            //            CMSPolicyName = c.String(),
            //            CMSPolicyDescription = c.String(),
            //            CMSPolicyType = c.String(),
            //            CMSValue = c.Decimal(precision: 18, scale: 2),
            //            IsUsed = c.Boolean(),
            //            isActive = c.Boolean(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.CustomerOfficer",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            OfficerID = c.Int(nullable: false),
            //            CustomerID = c.Int(),
            //            FullName = c.String(),
            //            Position = c.String(),
            //            PhoneNumber = c.String(),
            //            Email = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Customer", t => t.CustomerID)
            //    .Index(t => t.CustomerID);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Commission", "InvoiceID", "dbo.Invoice");
            //DropForeignKey("dbo.CustomerOfficer", "CustomerID", "dbo.Customer");
            //DropForeignKey("dbo.Commission", "CustomerOfficer_Id", "dbo.CustomerOfficer");
            //DropForeignKey("dbo.Commission", "CommissionPolicy_Id", "dbo.CommissionPolicy");
            //DropForeignKey("dbo.ACT_CommissionPayment", "CommissionID", "dbo.Commission");
            //DropIndex("dbo.CustomerOfficer", new[] { "CustomerID" });
            //DropIndex("dbo.Commission", new[] { "CustomerOfficer_Id" });
            //DropIndex("dbo.Commission", new[] { "CommissionPolicy_Id" });
            //DropIndex("dbo.Commission", new[] { "InvoiceID" });
            //DropIndex("dbo.ACT_CommissionPayment", new[] { "CommissionID" });
            //DropTable("dbo.CustomerOfficer");
            //DropTable("dbo.CommissionPolicy");
            //DropTable("dbo.Commission");
            //DropTable("dbo.ACT_CommissionPayment");
        }
    }
}
