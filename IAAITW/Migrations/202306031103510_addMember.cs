namespace IAAITW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMember : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 50),
                        Gender = c.Int(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        MemberTypes = c.Int(nullable: false),
                        Tel = c.String(nullable: false, maxLength: 10),
                        Mobile = c.String(nullable: false, maxLength: 10),
                        Address = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 200),
                        InternationalMembership = c.Boolean(nullable: false),
                        CurrentEmployer = c.String(nullable: false, maxLength: 50),
                        JobTitle = c.String(nullable: false, maxLength: 50),
                        HighestEducation = c.String(nullable: false, maxLength: 50),
                        PastEmployer1 = c.String(nullable: false, maxLength: 50),
                        PastJobTitle1 = c.String(nullable: false, maxLength: 50),
                        StartYear1 = c.Int(nullable: false),
                        StartMonth1 = c.Int(nullable: false),
                        EndYear1 = c.Int(nullable: false),
                        EndMonth1 = c.Int(nullable: false),
                        PastEmployer2 = c.String(maxLength: 50),
                        PastJobTitle2 = c.String(maxLength: 50),
                        StartYear2 = c.Int(),
                        StartMonth2 = c.Int(),
                        EndYear2 = c.Int(),
                        EndMonth2 = c.Int(),
                        PastEmployer3 = c.String(maxLength: 50),
                        PastJobTitle3 = c.String(maxLength: 50),
                        StartYear3 = c.Int(),
                        StartMonth3 = c.Int(),
                        EndYear3 = c.Int(),
                        EndMonth3 = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Members");
        }
    }
}
