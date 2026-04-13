namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_EmIncom : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.EmployeeIncome", "IncomeType");
            //RenameColumn(table: "dbo.EmployeeIncome", name: "CIncomeType_Id", newName: "IncomeType");
            //RenameIndex(table: "dbo.EmployeeIncome", name: "IX_CIncomeType_Id", newName: "IX_IncomeType");
            //CreateTable(
            //    "dbo.tblFileUpload",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            FName = c.String(),
            //            FPath = c.String(),
            //            CreatedDate = c.DateTime(),
            //            Catagory = c.String(),
            //            IsDeleted = c.Boolean(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.tblFileUpload");
            //RenameIndex(table: "dbo.EmployeeIncome", name: "IX_IncomeType", newName: "IX_CIncomeType_Id");
            //RenameColumn(table: "dbo.EmployeeIncome", name: "IncomeType", newName: "CIncomeType_Id");
            //AddColumn("dbo.EmployeeIncome", "IncomeType", c => c.Int());
        }
    }
}
