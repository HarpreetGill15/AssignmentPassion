namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class more : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Jerseys", "dbo.Orders");
            DropIndex("dbo.Jerseys", new[] { "Order_orderId" });
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jerseys", "Order_orderId", c => c.Int());
            CreateIndex("dbo.Jerseys", "Order_orderId");
            AddForeignKey("dbo.Jerseys", "Order_orderId", "dbo.Orders", "orderId");
        }
    }
}
