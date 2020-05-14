using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Web;
using System.Web.Configuration;

namespace AntTrak.Models
{
    public class Ticket
    {

        #region Ids
        public int Id { get; set; }
        public int ProjectId { get; set; }       
        public string SubmitterId { get; set; }
        public string DeveloperId { get; set; }
        public int TicketTypeId { get; set; }
        public int TicketStatusId { get; set; }
        public int TicketPriorityId { get; set; }

        #endregion
        #region Description
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public bool IsArchived { get; set; }

        #endregion
        #region Navigation
        public virtual Project Project { get; set; }
        public virtual ApplicationUser Submitter { get; set; }
        public virtual ApplicationUser Developer { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual ICollection<TicketAttachment> Attachments{get; set;}
        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketHistory> Histories { get; set; }
        public virtual ICollection<TicketNotification> Notifications { get; set; }
        #endregion

        public Ticket()
           
        {
            Attachments = new HashSet<TicketAttachment>();
            Comments = new HashSet<TicketComment>();
            Histories = new HashSet<TicketHistory>();
            Notifications = new HashSet<TicketNotification>();
        }


    }
}