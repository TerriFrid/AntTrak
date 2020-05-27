using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AntTrak.Models;
using AntTrak.ViewModel;

namespace AntTrak.ViewModel
{
    public class RoleManagement
    {
        //public CustomUserData CustomUser { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public int RoleId { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }


        //public RoleManagement()
        //{
        //    CustomUser = new CustomUserData();
           
        //}
    }

    
}