﻿using PassionProject.Data;
using PassionProject.Models;
using System;
using System.Collections.Generic;
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

    }
}