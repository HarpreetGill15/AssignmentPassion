using PassionProject.Data;
using PassionProject.Models;
using System;
using System.Collections.Generic;
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
    }
}