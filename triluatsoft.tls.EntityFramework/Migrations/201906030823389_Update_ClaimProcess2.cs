namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_ClaimProcess2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimProcess", "IsFirstSurvey", c => c.Boolean());
            AddColumn("dbo.ClaimProcess", "FirstSurveyTime", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "FirstSurveyBy", c => c.Int());
            AddColumn("dbo.ClaimProcess", "IsCompleteSurvey", c => c.Boolean());
            AddColumn("dbo.ClaimProcess", "CompleteSurveyTime", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "CompleteSurveyBy", c => c.Int());
            AddColumn("dbo.ClaimProcess", "IsRequestPREL1", c => c.Boolean());
            AddColumn("dbo.ClaimProcess", "RequestPREL1Time", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "RequestPREL1By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "IsRequestPREL2", c => c.Boolean());
            AddColumn("dbo.ClaimProcess", "RequestPREL2Time", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "RequestPREL2By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "IsConfirmPRE", c => c.Boolean());
            AddColumn("dbo.ClaimProcess", "ConfirmPRETime", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "ConfirmPREBy", c => c.Int());
            AddColumn("dbo.ClaimProcess", "IsRequestINTL1", c => c.Boolean());
            AddColumn("dbo.ClaimProcess", "RequestINTL1Time", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "RequestINTL1By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "IsRequestINTL2", c => c.Boolean());
            AddColumn("dbo.ClaimProcess", "RequestINTL2Time", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "RequestINTL2By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "IsConfirmINT", c => c.Boolean());
            AddColumn("dbo.ClaimProcess", "ConfirmINTTime", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "ConfirmINTBy", c => c.Int());
            AddColumn("dbo.ClaimProcess", "IsRequestFINL1", c => c.Boolean());
            AddColumn("dbo.ClaimProcess", "RequestFINL1Time", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "RequestFINL1By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "IsRequestFINL2", c => c.Boolean());
            AddColumn("dbo.ClaimProcess", "RequestFINL2Time", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "RequestFINL2By", c => c.Int());
            AddColumn("dbo.ClaimProcess", "IsConfirmFIN", c => c.Boolean());
            AddColumn("dbo.ClaimProcess", "ConfirmFINTime", c => c.DateTime());
            AddColumn("dbo.ClaimProcess", "ConfirmFINBy", c => c.Int());
            AlterColumn("dbo.ClaimProcess", "IsAck", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsDocRequest", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsMeetingNote", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsILA", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsPREL1", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsPREL2", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsPREHC", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsINTL1", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsINTL2", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsINTHC", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsFINL1", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsFINL2", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsFINHC", c => c.Boolean());
            AlterColumn("dbo.ClaimProcess", "IsSubmit", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClaimProcess", "IsSubmit", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsFINHC", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsFINL2", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsFINL1", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsINTHC", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsINTL2", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsINTL1", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsPREHC", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsPREL2", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsPREL1", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsILA", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsMeetingNote", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsDocRequest", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ClaimProcess", "IsAck", c => c.Boolean(nullable: false));
            DropColumn("dbo.ClaimProcess", "ConfirmFINBy");
            DropColumn("dbo.ClaimProcess", "ConfirmFINTime");
            DropColumn("dbo.ClaimProcess", "IsConfirmFIN");
            DropColumn("dbo.ClaimProcess", "RequestFINL2By");
            DropColumn("dbo.ClaimProcess", "RequestFINL2Time");
            DropColumn("dbo.ClaimProcess", "IsRequestFINL2");
            DropColumn("dbo.ClaimProcess", "RequestFINL1By");
            DropColumn("dbo.ClaimProcess", "RequestFINL1Time");
            DropColumn("dbo.ClaimProcess", "IsRequestFINL1");
            DropColumn("dbo.ClaimProcess", "ConfirmINTBy");
            DropColumn("dbo.ClaimProcess", "ConfirmINTTime");
            DropColumn("dbo.ClaimProcess", "IsConfirmINT");
            DropColumn("dbo.ClaimProcess", "RequestINTL2By");
            DropColumn("dbo.ClaimProcess", "RequestINTL2Time");
            DropColumn("dbo.ClaimProcess", "IsRequestINTL2");
            DropColumn("dbo.ClaimProcess", "RequestINTL1By");
            DropColumn("dbo.ClaimProcess", "RequestINTL1Time");
            DropColumn("dbo.ClaimProcess", "IsRequestINTL1");
            DropColumn("dbo.ClaimProcess", "ConfirmPREBy");
            DropColumn("dbo.ClaimProcess", "ConfirmPRETime");
            DropColumn("dbo.ClaimProcess", "IsConfirmPRE");
            DropColumn("dbo.ClaimProcess", "RequestPREL2By");
            DropColumn("dbo.ClaimProcess", "RequestPREL2Time");
            DropColumn("dbo.ClaimProcess", "IsRequestPREL2");
            DropColumn("dbo.ClaimProcess", "RequestPREL1By");
            DropColumn("dbo.ClaimProcess", "RequestPREL1Time");
            DropColumn("dbo.ClaimProcess", "IsRequestPREL1");
            DropColumn("dbo.ClaimProcess", "CompleteSurveyBy");
            DropColumn("dbo.ClaimProcess", "CompleteSurveyTime");
            DropColumn("dbo.ClaimProcess", "IsCompleteSurvey");
            DropColumn("dbo.ClaimProcess", "FirstSurveyBy");
            DropColumn("dbo.ClaimProcess", "FirstSurveyTime");
            DropColumn("dbo.ClaimProcess", "IsFirstSurvey");
        }
    }
}
