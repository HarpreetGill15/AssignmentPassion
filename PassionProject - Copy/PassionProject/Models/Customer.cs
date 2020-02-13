using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject.Models
{
    public class Customer
    {
        //data needed for a customer 
        //id, first and last name, email, phone number, address
        //maybe use postal code and country to calculate shipping
        [Key]
        public int customerId { get; set; }
        public string customerFirstName { get; set; }
        public string customerLastName { get; set; }
        public string customerEmail { get; set; }
        public string customerPhone { get; set; }
        public string customerAddress { get; set; }
        public string customerPostalCode { get; set; }
        public string customerCountry { get; set; }
    }
}