namespace triluatsoft.tls.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_ClaimProcess : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClaimProcess",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsAck = c.Boolean(nullable: false),
                        AckDeadline = c.DateTime(),
                        AckTime = c.DateTime(),
                        IsMeetingNote = c.Boolean(nullable: false),
                        MeetingNoteDeadline = c.DateTime(),
                        MeetingNoteTime = c.DateTime(),
                        IsILA = c.Boolean(nullable: false),
                        ILADeadline = c.DateTime(),
                        ILATime = c.DateTime(),
                        IsPREL1 = c.Boolean(nullable: false),
                        PREL1Deadline = c.DateTime(),
                        PREL1Time = c.DateTime(),
                        IsPREL2 = c.Boolean(nullable: false),
                        PREL2Deadline = c.DateTime(),
                        PREL2Time = c.DateTime(),
                        IsPREHC = c.Boolean(nullable: false),
                        PREHCDeadline = c.DateTime(),
                        PREHCTime = c.DateTime(),
                        IsINTL1 = c.Boolean(nullable: false),
                        INTL1Deadline = c.DateTime(),
                        INTL1Time = c.DateTime(),
                        IsINTL2 = c.Boolean(nullable: false),
                        INTL2Deadline = c.DateTime(),
                        INTL2Time = c.DateTime(),
                        IsINTHC = c.Boolean(nullable: false),
                        INTHCDeadline = c.DateTime(),
                        INTHCTime = c.DateTime(),
                        IsFINL1 = c.Boolean(nullable: false),
                        FINL1Deadline = c.DateTime(),
                        FINL1Time = c.DateTime(),
                        IsFINL2 = c.Boolean(nullable: false),
                        FINL2Deadline = c.DateTime(),
                        FINL2Time = c.DateTime(),
                        IsFINHC = c.Boolean(nullable: false),
                        FINHCDeadline = c.DateTime(),
                        FINHCTime = c.DateTime(),
                        IsSubmit = c.Boolean(nullable: false),
                        SubmitDeadline = c.DateTime(),
                        SubmitTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClaimProcess");
        }
    }
}
