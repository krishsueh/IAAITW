namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amendAboutsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Abouts", "Organization", c => c.String(maxLength: 4000));
            AddColumn("dbo.Abouts", "History", c => c.String(maxLength: 4000));
            AddColumn("dbo.Abouts", "LicensedMember", c => c.String(maxLength: 4000));
            AddColumn("dbo.Abouts", "Expert", c => c.String(maxLength: 4000));
            DropColumn("dbo.Abouts", "InitDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Abouts", "InitDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Abouts", "Expert");
            DropColumn("dbo.Abouts", "LicensedMember");
            DropColumn("dbo.Abouts", "History");
            DropColumn("dbo.Abouts", "Organization");
        }
    }
}
