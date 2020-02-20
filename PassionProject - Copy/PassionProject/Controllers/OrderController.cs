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
        //Order/List
        public ActionResult List()
        {
            List<Order> orders = db.Orders.SqlQuery("Select * from Orders").ToList();
            return View(orders);
        }
        //Order/Show/int
        public ActionResult Show(int? id)
        {
            //check if user lands on page without a given id
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest,"order does not exist");
            }
            //get the order with the id that is passed
            Order order = db.Orders.SqlQuery("select * from Orders where orderid=@OrderId", new SqlParameter("@OrderId", id)).FirstOrDefault();
            if (order == null)
            {
                return HttpNotFound();
            }
            //also need the jerseys for that order
            //select * from jerseys and inner join with orderjerseys on jersey id where order id = passed value
            string query = "select * from Jerseys inner join OrderJerseys on Jerseys.jerseyId = OrderJerseys.Jersey_jerseyId where OrderJerseys.Order_orderId = @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            List<Jersey> jerseyOrders = db.Jerseys.SqlQuery(query, parameter).ToList();

            //get list of all jerseys
            string alljerseys = "select * from Jerseys";
            List<Jersey> jerseys = db.Jerseys.SqlQuery(alljerseys).ToList();

            //output information of that order
            //output inforamtion of all jerseys in that order
            //output the list of all jerseys to populate the dropdown
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
                //update the stock of a the jersey
                ////get the stock of the jersey
                List<Jersey> jerseyList = db.Jerseys.SqlQuery("select * from jerseys where jerseyId = @id", new SqlParameter("@id", jerseyId)).ToList();
                //get the jersey stock int from the jersey list and add it to an int
                int stock = jerseyList.First().jerseyStock - 1;
                //update the stock to be stock - 1
                string updateStock = "update jerseys set jerseyStock=@stock where jerseyId =@id";
                SqlParameter[] sqlParameters = new SqlParameter[2];
                sqlParameters[0] = new SqlParameter("@stock", stock);
                sqlParameters[1] = new SqlParameter("@id", jerseyId);

                Debug.WriteLine("updating jersey id " + jerseyId + " so that the stock lowers from " + jerseyList.First().jerseyStock + " to " + stock);
                int count = db.Database.ExecuteSqlCommand(updateStock, sqlParameters);

                    //insert the order id and jersey id into the orderJerseys table
                    string query = "insert into OrderJerseys (Order_orderId, Jersey_jerseyId) values (@orderId,@jerseyId)";
                    SqlParameter[] sqlParameter = new SqlParameter[2];
                    sqlParameter[0] = new SqlParameter("@orderId", id);
                    sqlParameter[1] = new SqlParameter("@jerseyid", jerseyId);

                    //execute command 
                    db.Database.ExecuteSqlCommand(query, sqlParameter);

            }
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
            //update the stock of a the jersey
            ////get the stock of the jersey
            List<Jersey> jerseyList = db.Jerseys.SqlQuery("select * from jerseys where jerseyId = @id", new SqlParameter("@id", jerseyId)).ToList();
            //get the jersey stock int from the jersey list and add it to an int
            int stock = jerseyList.First().jerseyStock + 1;
            //update the stock to be stock + 1
            string updateStock = "update jerseys set jerseyStock=@stock where jerseyId =@id";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@stock", stock);
            sqlParameters[1] = new SqlParameter("@id", jerseyId);

            Debug.WriteLine("updating jersey id " + jerseyId + " so that the stock goes from " + jerseyList.First().jerseyStock + " to " + stock);
            db.Database.ExecuteSqlCommand(updateStock, sqlParameters);
            return RedirectToAction("Show/" + id);
        }
        //Order/Add
        public ActionResult Add()
        {
            ////get list of all jerseys
            //string alljerseys = "select * from Jerseys";
            //List<Jersey> jerseys = db.Jerseys.SqlQuery(alljerseys).ToList();

            //get list of all Customers
            string allCustomer = "select * from Customers";
            List<Customer> customer = db.Customers.SqlQuery(allCustomer).ToList();

            return View(customer);
        }
        //Order/Add/int
        [HttpPost]
        public ActionResult Add(int customerId)
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
        //Order/DeleteOrder/int
        public ActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.SqlQuery("select * from Orders where orderId = @id", new SqlParameter("@id", id)).FirstOrDefault();

            string query = "select * from Jerseys inner join OrderJerseys on Jerseys.jerseyId = OrderJerseys.Jersey_jerseyId where OrderJerseys.Order_orderId = @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            List<Jersey> jerseyOrders = db.Jerseys.SqlQuery(query, parameter).ToList();

            //output the order information to the view
            //output all the jerseys in that order
            ShowOrder showOrder = new ShowOrder();
            showOrder.ord = order;
            showOrder.jerseys = jerseyOrders;

            return View(showOrder);
        }
        public ActionResult Delete(int id)
        {
            Debug.WriteLine("trying to delete order with the id of " + id);
            //delete from orders
            string query = "delete from Orders where orderId= @id";
            SqlParameter parameter = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, parameter);

            //also delete from bridging table with any jerseys in that order
            string query2 = "delete from OrderJerseys where Order_orderId=@id";
            SqlParameter parameter2 = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query2, parameter2);

            return RedirectToAction("List");
        }
    }
}