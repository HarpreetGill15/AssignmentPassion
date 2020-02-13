namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        orderId = c.Int(nullable: false, identity: true),
                        customerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.orderId)
                .ForeignKey("dbo.Customers", t => t.customerId, cascadeDelete: true)
                .Index(t => t.customerId);
            
            AddColumn("dbo.Jerseys", "Order_orderId", c => c.Int());
            CreateIndex("dbo.Jerseys", "Order_orderId");
            AddForeignKey("dbo.Jerseys", "Order_orderId", "dbo.Orders", "orderId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jerseys", "Order_orderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "customerId", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "customerId" });
            DropIndex("dbo.Jerseys", new[] { "Order_orderId" });
            DropColumn("dbo.Jerseys", "Order_orderId");
            DropTable("dbo.Orders");
        }
    }
}
