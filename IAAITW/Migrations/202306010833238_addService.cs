namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addService : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Jobs = c.String(maxLength: 3000),
                        Licenses = c.String(maxLength: 3000),
                        Refer = c.String(maxLength: 3000),
                        Survey = c.String(maxLength: 3000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Services");
        }
    }
}
