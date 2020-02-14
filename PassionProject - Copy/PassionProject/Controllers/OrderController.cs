using PassionProject.Data;
using PassionProject.Models;
using PassionProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PassionProject.Controllers
{
    public class OrderController : Controller
    {
        private ShopContext db = new ShopContext();
        // GET: Order
        public ActionResult List()
        {
            List<Order> orders = db.Orders.SqlQuery("Select * from Orders").ToList();
            return View(orders);
        }
        public ActionResult Show(int id)
        {
            //get the order with the id that is passed
            Order order = db.Orders.SqlQuery("select * from Orders where orderid=@OrderId", new SqlParameter("@OrderId", id)).FirstOrDefault();
            
            //also need the jerseys for that order
            //select * from jerseys and inner join with orderjerseys on jersey id where order id = passed value
            string query = "select * from Jerseys inner join OrderJerseys on Jerseys.jerseyId = OrderJerseys.Jersey_jerseyId where Order_orderId = @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            List<Jersey> jerseyOrders = db.Jerseys.SqlQuery(query, parameter).ToList();

            //get list of all jerseys
            string alljerseys = "select * from Jerseys";
            List<Jersey> jerseys = db.Jerseys.SqlQuery(alljerseys).ToList();

            ShowOrder showOrder = new ShowOrder();
            showOrder.ord = order;
            showOrder.jerseys = jerseyOrders;
            showOrder.allJerseys = jerseys;

            return View(showOrder);
        }
        //to add a jersey to an order
        [HttpPost]
        public ActionResult AddJersey(int id, int jerseyId)
        {
                Debug.WriteLine("Order is " + id + " and jerseyId is " + jerseyId+" and price is ");

                //insert the order id and jersey id into the orderJerseys table
                string query = "insert into OrderJerseys (Order_orderId, Jersey_jerseyId) values (@orderId,@jerseyId)";
                SqlParameter[] sqlParameter = new SqlParameter[2];
                sqlParameter[0] = new SqlParameter("@orderId", id);
                sqlParameter[1] = new SqlParameter("@jerseyid", jerseyId);
                
                //execute command 
                db.Database.ExecuteSqlCommand(query, sqlParameter);

                //update price of order
                //string priceQuery = "update Orders set orderPrice=@price where orderid=@orderId";
                //SqlParameter[] sql = new SqlParameter[2];
                //sql[0] = new SqlParameter("@orderId", id);
                //sql[1] = new SqlParameter("@price", price);

                //ececute update price command
                //db.Database.ExecuteSqlCommand(priceQuery, sql);
            return RedirectToAction("Show/"+id);
        }
        //remove jersey from order
        [HttpGet]
        public ActionResult RemoveJersey(int id, int jerseyId)
        {
            Debug.WriteLine("Order is " + id + " and jerseyId is " + jerseyId);

            //insert the order id and jersey id into the orderJerseys table
            string query = "delete from OrderJerseys where Order_orderId=@orderId and Jersey_jerseyId=@jerseyId";
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@orderId", id);
            sqlParameter[1] = new SqlParameter("@jerseyid", jerseyId);

            //execute command 
            db.Database.ExecuteSqlCommand(query, sqlParameter);
            //update price of order
            //string priceQuery = "update Orders set orderPrice=@price where orderid=@orderId";
            //SqlParameter[] sql = new SqlParameter[2];
            //sql[0] = new SqlParameter("@orderId", id);
            //sql[1] = new SqlParameter("@price", price);

            ////ececute update price command
            //db.Database.ExecuteSqlCommand(priceQuery, sql);
            return RedirectToAction("Show/" + id);
        }
    }
}