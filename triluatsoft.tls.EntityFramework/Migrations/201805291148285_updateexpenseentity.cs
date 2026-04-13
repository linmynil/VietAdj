namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateexpenseentity : DbMigration
    {
        public override void Up()
        {
            //CreateIndex("dbo.Expense", "ExpenseTypeID");
            //AddForeignKey("dbo.Expense", "ExpenseTypeID", "dbo.ExpenseType", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Expense", "ExpenseTypeID", "dbo.ExpenseType");
            //DropIndex("dbo.Expense", new[] { "ExpenseTypeID" });
        }
    }
}
