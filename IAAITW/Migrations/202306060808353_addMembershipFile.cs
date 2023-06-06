namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMembershipFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "MembershipFile", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "MembershipFile");
        }
    }
}
