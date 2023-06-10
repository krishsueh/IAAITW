namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amendPartner : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Partners", "Image", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Partners", "Image", c => c.String(maxLength: 100));
        }
    }
}
