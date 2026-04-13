namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_borderaux_col_recentCorrespondence : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bordereaux", "RecentCorrespondence", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bordereaux", "RecentCorrespondence");
        }
    }
}
