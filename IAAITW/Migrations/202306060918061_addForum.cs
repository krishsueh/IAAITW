namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addForum : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.ReplyId)
                .ForeignKey("dbo.Forums", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Forums",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Author = c.String(),
                        Content = c.String(maxLength: 3000),
                        PostDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SubjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumReplies", "SubjectId", "dbo.Forums");
            DropIndex("dbo.ForumReplies", new[] { "SubjectId" });
            DropTable("dbo.Forums");
            DropTable("dbo.ForumReplies");
        }
    }
}
