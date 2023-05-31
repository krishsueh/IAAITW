namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCMSAccountAndAbout : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abouts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AboutUs = c.String(maxLength: 4000),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMSAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Headshot = c.String(maxLength: 200),
                        Email = c.String(nullable: false, maxLength: 200),
                        Password = c.String(maxLength: 200),
                        IdentityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CMSIdentities", t => t.IdentityId, cascadeDelete: true)
                .Index(t => t.IdentityId);
            
            CreateTable(
                "dbo.CMSIdentities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identity = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CMSAccounts", "IdentityId", "dbo.CMSIdentities");
            DropIndex("dbo.CMSAccounts", new[] { "IdentityId" });
            DropTable("dbo.CMSIdentities");
            DropTable("dbo.CMSAccounts");
            DropTable("dbo.Abouts");
        }
    }
}
