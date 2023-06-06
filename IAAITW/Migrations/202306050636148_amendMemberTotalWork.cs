namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amendMemberTotalWork : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "TotalYear", c => c.Int(nullable: false));
            AddColumn("dbo.Members", "TotalMonth", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "TotalMonth");
            DropColumn("dbo.Members", "TotalYear");
        }
    }
}
