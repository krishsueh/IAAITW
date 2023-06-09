namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resetGoTop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "GoTop", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "GoTop");
        }
    }
}
