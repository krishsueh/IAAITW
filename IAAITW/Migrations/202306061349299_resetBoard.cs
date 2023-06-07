namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resetBoard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardReplies",
                c => new
                    {
                        ReplyId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        Replyer = c.String(maxLength: 50),
                        ReplyArticle = c.String(maxLength: 3000),
                        ReplyDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReplyId)
                .ForeignKey("dbo.Boards", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Boards",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        Subject = c.String(maxLength: 50),
                        Author = c.String(maxLength: 50),
                        Article = c.String(maxLength: 3000),
                        PostDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SubjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoardReplies", "SubjectId", "dbo.Boards");
            DropIndex("dbo.BoardReplies", new[] { "SubjectId" });
            DropTable("dbo.Boards");
            DropTable("dbo.BoardReplies");
        }
    }
}
