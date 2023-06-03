namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKnowledge : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Knowledges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReleaseDate = c.DateTime(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(nullable: false, maxLength: 3000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Knowledges");
        }
    }
}
