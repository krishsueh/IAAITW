namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delMaxLengthInAbouts : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Abouts", "AboutUs", c => c.String());
            AlterColumn("dbo.Abouts", "Organization", c => c.String());
            AlterColumn("dbo.Abouts", "History", c => c.String());
            AlterColumn("dbo.Abouts", "LicensedMember", c => c.String());
            AlterColumn("dbo.Abouts", "Expert", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Abouts", "Expert", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Abouts", "LicensedMember", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Abouts", "History", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Abouts", "Organization", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Abouts", "AboutUs", c => c.String(maxLength: 4000));
        }
    }
}
