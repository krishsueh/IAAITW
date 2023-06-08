namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeMaxLenth : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Knowledges", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.News", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "Content", c => c.String(nullable: false, maxLength: 3000));
            AlterColumn("dbo.Knowledges", "Content", c => c.String(nullable: false, maxLength: 3000));
        }
    }
}
