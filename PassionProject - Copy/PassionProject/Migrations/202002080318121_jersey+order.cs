namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jerseyorder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jerseys",
                c => new
                    {
                        jerseyId = c.Int(nullable: false, identity: true),
                        jerseyName = c.String(),
                        jerseySize = c.String(),
                        jerseyDescription = c.String(),
                        jerseyPrice = c.Double(nullable: false),
                        jerseyStock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.jerseyId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Jerseys");
        }
    }
}
