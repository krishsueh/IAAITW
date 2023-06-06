namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amendMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Password2", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "Password2");
        }
    }
}
