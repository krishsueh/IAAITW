namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKnowledgeCover : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Knowledges", "CoverImg", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Knowledges", "CoverImg");
        }
    }
}
