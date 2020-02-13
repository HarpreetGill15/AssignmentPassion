using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject.Models
{
    public class Jersey
    {
        //data needed for a jersey
        //id, name of jersey, size, description, image, price, stock
        [Key]
        public int jerseyId { get; set; }
        public string jerseyName { get; set; }
        public string jerseySize { get; set; }
        public string jerseyDescription { get; set; }
        public double jerseyPrice { get; set; }
        public int jerseyStock { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}