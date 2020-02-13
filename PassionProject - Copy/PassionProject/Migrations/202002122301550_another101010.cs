namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class another101010 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderJerseys",
                c => new
                    {
                        Order_orderId = c.Int(nullable: false),
                        Jersey_jerseyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Order_orderId, t.Jersey_jerseyId })
                .ForeignKey("dbo.Orders", t => t.Order_orderId, cascadeDelete: true)
                .ForeignKey("dbo.Jerseys", t => t.Jersey_jerseyId, cascadeDelete: true)
                .Index(t => t.Order_orderId)
                .Index(t => t.Jersey_jerseyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderJerseys", "Jersey_jerseyId", "dbo.Jerseys");
            DropForeignKey("dbo.OrderJerseys", "Order_orderId", "dbo.Orders");
            DropIndex("dbo.OrderJerseys", new[] { "Jersey_jerseyId" });
            DropIndex("dbo.OrderJerseys", new[] { "Order_orderId" });
            DropTable("dbo.OrderJerseys");
        }
    }
}
