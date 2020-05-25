using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AntTrak.Models;
using AntTrak.ViewModel;

namespace AntTrak.ViewModel
{
    public class RoleManagement
    {
        public CustomUserData customUser { get; set; }
        public int RoleId { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}