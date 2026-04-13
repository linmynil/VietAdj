namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updated_ClaimProcess : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimProcess", "AckBy", c => c.Int());
            AddColumn("dbo.ClaimProcess", "MeetingNoteBy", c => c.Int());
            AddColumn("dbo.ClaimProcess", "ILABy", c => c.Int());
            AddColumn("dbo.ClaimProcess", "PREL1By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "PREL2By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "PREHCBy", c => c.Int());
            AddColumn("dbo.ClaimProcess", "INTL1By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "INTL2By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "INTHCBy", c => c.Int());
            AddColumn("dbo.ClaimProcess", "FINL1By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "FINL2By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "FINHCBy", c => c.Int());
            AddColumn("dbo.ClaimProcess", "SubmitBy", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClaimProcess", "SubmitBy");
            DropColumn("dbo.ClaimProcess", "FINHCBy");
            DropColumn("dbo.ClaimProcess", "FINL2By");
            DropColumn("dbo.ClaimProcess", "FINL1By");
            DropColumn("dbo.ClaimProcess", "INTHCBy");
            DropColumn("dbo.ClaimProcess", "INTL2By");
            DropColumn("dbo.ClaimProcess", "INTL1By");
            DropColumn("dbo.ClaimProcess", "PREHCBy");
            DropColumn("dbo.ClaimProcess", "PREL2By");
            DropColumn("dbo.ClaimProcess", "PREL1By");
            DropColumn("dbo.ClaimProcess", "ILABy");
            DropColumn("dbo.ClaimProcess", "MeetingNoteBy");
            DropColumn("dbo.ClaimProcess", "AckBy");
        }
    }
}
