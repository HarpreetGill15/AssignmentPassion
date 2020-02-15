using PassionProject.Data;
using PassionProject.Models;
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
        public ActionResult ListJerseys()
        {
            List<Jersey> jerseys = db.Jerseys.SqlQuery("Select * from Jerseys").ToList();
            return View(jerseys);
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
    }
}