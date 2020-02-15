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
            string query = "select * from Jerseys inner join OrderJerseys on Jerseys.jerseyId = OrderJerseys.Jersey_jerseyId where OrderJerseys.Order_orderId = @id";
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
            //check if the jersey is already in the order
            string check = "select * from jerseys inner join OrderJerseys on OrderJerseys.Jersey_jerseyid = jerseys.jerseyId where Jersey_jerseyid = @id and OrderJerseys.Order_orderid = @orderId";
            SqlParameter[] sql = new SqlParameter[2];
            sql[0] = new SqlParameter("@id", jerseyId);
            sql[1] = new SqlParameter("@orderId", id);
            List<Jersey> list = db.Jerseys.SqlQuery(check, sql).ToList();
            if(list.Count <= 0)
            {
                //insert the order id and jersey id into the orderJerseys table
                string query = "insert into OrderJerseys (Order_orderId, Jersey_jerseyId) values (@orderId,@jerseyId)";
                SqlParameter[] sqlParameter = new SqlParameter[2];
                sqlParameter[0] = new SqlParameter("@orderId", id);
                sqlParameter[1] = new SqlParameter("@jerseyid", jerseyId);

                //execute command 
                db.Database.ExecuteSqlCommand(query, sqlParameter);

               
            }
                
                

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
        public ActionResult Add()
        {
            //get list of all jerseys
            string alljerseys = "select * from Jerseys";
            List<Jersey> jerseys = db.Jerseys.SqlQuery(alljerseys).ToList();

            //get list of all jerseys
            string allCustomer = "select * from Customers";
            List<Customer> customer = db.Customers.SqlQuery(allCustomer).ToList();

            //get order id
            //string getid = "select max(orderId) from Orders";
            //Order order = db.Orders.SqlQuery(getid).First();

            AddOrder addOrder = new AddOrder();
            addOrder.Jerseys = jerseys;
            addOrder.Customers = customer;
            return View(addOrder);
        }
        [HttpPost]
        public ActionResult Add(int customerId,int jerseyId)
        {
            //add to orders
            string query = "insert into Orders (customerid,orderDate) values(@customerId,@date)";
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@customerId", customerId);
            sqlParameter[1] = new SqlParameter("@date", DateTime.Now);

            
            try
            {
                db.Database.ExecuteSqlCommand(query, sqlParameter);
            }
            catch (Exception e)
            {
                //write the error to the console
                Debug.WriteLine(e);
                return RedirectToAction("List");
            }

            
            return RedirectToAction("List");
        }
        //when order is complete return to orders list 
        //make it so the order cannot be edited later
        [HttpPost]
        public ActionResult CompleteOrder(int id, double sum)
        {
            Debug.WriteLine("The order is " + id + " and the total is " + sum);
            //update the order with the total price
            string query = "update Orders set orderPrice=@sum where orderId = @orderId";
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@orderId", id);
            sqlParameter[1] = new SqlParameter("@sum", sum);
            

            db.Database.ExecuteSqlCommand(query, sqlParameter);

            return RedirectToAction("List");
        }
    }
}