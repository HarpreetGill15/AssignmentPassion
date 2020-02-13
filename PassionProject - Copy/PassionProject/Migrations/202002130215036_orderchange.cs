namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderchange : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Orders", new[] { "customerId" });
            AddColumn("dbo.Orders", "orderDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "orderPrice", c => c.Double(nullable: false));
            CreateIndex("dbo.Orders", "CustomerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropColumn("dbo.Orders", "orderPrice");
            DropColumn("dbo.Orders", "orderDate");
            CreateIndex("dbo.Orders", "customerId");
        }
    }
}
