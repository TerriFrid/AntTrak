using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AntTrak.Models;

namespace AntTrak.ViewModel
{
    public class ProjectDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectManagerId { get; set; }

        public string ProjectManagerName { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? Updated { get; set; }

        public bool IsArchived { get; set; }

        public List<Ticket> ActiveTickets { get; set; }
        public List<Ticket> ClosedTickets { get; set; }

        public List<ApplicationUser> ProjectMembers { get; set; }
    }
}