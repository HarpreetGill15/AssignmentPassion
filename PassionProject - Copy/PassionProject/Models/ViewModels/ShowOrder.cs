using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class ShowOrder
    {
        
        //One order
        public virtual Order ord{ get; set; }

        //a list of all the jerseys in that order
        public List<Jersey> jerseys { get; set; }

        //list of all jersey for dropdown to add to the order
        public List<Jersey> allJerseys { get; set; }
    }
}