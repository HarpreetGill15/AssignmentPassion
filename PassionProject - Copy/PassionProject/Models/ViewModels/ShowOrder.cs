using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class ShowOrder
    {
        public virtual Order ord{ get; set; }

        public List<Jersey> jerseys { get; set; }
    }
}