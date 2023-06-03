namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewsCover : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "CoverImg", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "CoverImg");
        }
    }
}
