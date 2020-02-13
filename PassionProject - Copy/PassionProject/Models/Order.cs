using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassionProject.Models
{
    public class Order
    {
        //data needed for orders
        //id, userid, useraddress, email, maybe phone, jerseyid, jerseysize,jersey price
        [Key]
        public int orderId { get; set; }

        public DateTime orderDate { get; set; }
        public double orderPrice { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        

        public ICollection<Jersey> Jerseys { get; set; }
    }
}