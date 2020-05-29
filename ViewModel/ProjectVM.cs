using AntTrak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntTrak.ViewModel
{
    public class ProjectVM
    {
        public int id { get; set;  }
        public string name { get; set; }
        public string pmId { get; set; }
        public string pmName { get; set; }
        public string description { get; set; }


        public List<ApplicationUser> ProjectMembers { get; set; }
    }
}