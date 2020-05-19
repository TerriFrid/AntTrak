using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AntTrak.ViewModel
{
    public class TicketDetail
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? Updated { get; set; }

        public bool IsArchived { get; set; }

       public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string SubmitterId { get; set; }
        public string SubmitterName { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public int TicketStatusId { get; set; }
        public string TicketStatusName { get; set; }
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public string DeveloperId { get; set; }
        public string DeveloperName { get; set; } 


    }
}