using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class ShowCustomer
    {
        //one customer
        public virtual Customer Customer { get; set; }
        //many orders
        public List<Order> orders { get; set; }
        //many jerseys
        public List<Jersey> jerseys { get; set; }

    }
}