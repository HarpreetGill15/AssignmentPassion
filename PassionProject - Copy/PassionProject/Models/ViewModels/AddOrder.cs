using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class AddOrder
    {
        //add order view requires customers, jerseys, orders

        //one order
        public virtual Order Order { get; set; }
        //list of customers
        public List<Customer> Customers { get; set; }
        //list of jerseys
        public List<Jersey> Jerseys { get; set; }
    }
}