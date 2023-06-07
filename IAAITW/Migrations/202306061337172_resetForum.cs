namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resetForum : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ForumReplies", "SubjectId", "dbo.Fora");
            DropIndex("dbo.ForumReplies", new[] { "SubjectId" });
            DropTable("dbo.ForumReplies");
            DropTable("dbo.Fora");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Fora",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Author = c.String(),
                        Content = c.String(maxLength: 3000),
                        PostDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SubjectId);
            
            CreateTable(
                "dbo.ForumReplies",
                c => new
                    {
                        ReplyId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        Replyer = c.String(),
                        Content = c.String(maxLength: 3000),
                        ReplyDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReplyId);
            
            CreateIndex("dbo.ForumReplies", "SubjectId");
            AddForeignKey("dbo.ForumReplies", "SubjectId", "dbo.Fora", "SubjectId", cascadeDelete: true);
        }
    }
}
