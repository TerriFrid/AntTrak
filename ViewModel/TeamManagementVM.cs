using AntTrak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AntTrak.ViewModel
{
    public class TeamManagementVM
    {
        public Project Project { get; set; }
        public IEnumerable<SelectListItem> Submitters { get; set; }
        public IEnumerable<SelectListItem> Developers { get; set; }
    }
}