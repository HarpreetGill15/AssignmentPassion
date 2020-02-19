using PassionProject.Data;
using PassionProject.Models;
using PassionProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PassionProject.Controllers
{
    public class CustomerController : Controller
    {
        private ShopContext db = new ShopContext();
        // GET: Customer
        public ActionResult List()
        {
            List<Customer> customers = db.Customers.SqlQuery("Select * from Customers").ToList();
            return View(customers);
        }
        //add customer 
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string customerFirstName, string customerLastName, string customerEmail, string customerPhone, string customerAddress, string customerPostalCode, string customerCountry)
        {
            string query = "insert into Customers (customerFirstName, customerLastName, customerEmail, customerPhone, customerAddress, customerPostalCode, customerCountry) values (@fname,@lname,@email,@phone,@address,@postal,@country)";
            SqlParameter[] sqlParameters = new SqlParameter[7];
            sqlParameters[0] = new SqlParameter("@fname", customerFirstName);
            sqlParameters[1] = new SqlParameter("@lname", customerLastName);
            sqlParameters[2] = new SqlParameter("@email", customerEmail);
            sqlParameters[3] = new SqlParameter("@phone", customerPhone);
            sqlParameters[4] = new SqlParameter("@address", customerAddress);
            sqlParameters[5] = new SqlParameter("@postal", customerPostalCode);
            sqlParameters[6] = new SqlParameter("@country", customerCountry);

            db.Database.ExecuteSqlCommand(query, sqlParameters);

            return RedirectToAction("List");
        }
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.SqlQuery("select * from Customers where customerId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }
            //show list of orders of that customer
            List<Order> orders = db.Orders.SqlQuery("Select * from Orders where customerId=@id", new SqlParameter("@id",id)).ToList();
            //show the jerseys this customer ordered

            ShowCustomer show = new ShowCustomer();
            show.Customer = customer;
            show.orders = orders;
            return View(show);
        }
        public ActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.SqlQuery("select * from Customers where customerId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            return View(customer);
        }
        public ActionResult Delete(int id)
        {
            string query = "delete from Customers where customerId= @id";
            SqlParameter parameter = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, parameter);

            return RedirectToAction("List");
        }
    }
}