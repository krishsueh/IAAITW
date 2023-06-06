namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amendMenber : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Members", "Password2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "Password2", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
