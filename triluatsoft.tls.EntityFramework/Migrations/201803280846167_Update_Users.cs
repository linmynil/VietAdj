namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Users : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "JobTitle", c => c.String());
            AddColumn("dbo.AbpUsers", "JobPosition", c => c.String());
            AddColumn("dbo.AbpUsers", "Address", c => c.String());
            AddColumn("dbo.AbpUsers", "Phone", c => c.String());
            AddColumn("dbo.AbpUsers", "JoinDate", c => c.DateTime());
            AddColumn("dbo.AbpUsers", "DateOfBirth", c => c.DateTime());
            AddColumn("dbo.AbpUsers", "Fee", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "Fee");
            DropColumn("dbo.AbpUsers", "DateOfBirth");
            DropColumn("dbo.AbpUsers", "JoinDate");
            DropColumn("dbo.AbpUsers", "Phone");
            DropColumn("dbo.AbpUsers", "Address");
            DropColumn("dbo.AbpUsers", "JobPosition");
            DropColumn("dbo.AbpUsers", "JobTitle");
        }
    }
}
