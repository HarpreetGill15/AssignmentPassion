namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        customerId = c.Int(nullable: false, identity: true),
                        customerFirstName = c.String(),
                        customerLastName = c.String(),
                        customerEmail = c.String(),
                        customerPhone = c.String(),
                        customerAddress = c.String(),
                        customerPostalCode = c.String(),
                        customerCountry = c.String(),
                    })
                .PrimaryKey(t => t.customerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Customers");
        }
    }
}
