using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PassionProject.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext() : base("name=ShopContext")
        {
            
        }
        public System.Data.Entity.DbSet<PassionProject.Models.Customer> Customers { get; set; }
        public System.Data.Entity.DbSet<PassionProject.Models.Jersey> Jerseys { get; set; }
        public System.Data.Entity.DbSet<PassionProject.Models.Order> Orders { get; set; }
    }
}