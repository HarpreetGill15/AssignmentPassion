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

        public int customerId { get; set; }
        [ForeignKey("customerId")]
        public virtual Customer Customer { get; set; }

        

        public ICollection<Jersey> Jerseys { get; set; }
    }
}