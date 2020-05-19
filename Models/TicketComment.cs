using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AntTrak.Models
{
    public class TicketComment
    {
        #region Ids
        public int Id { get; set; }
        public int TicketId {get; set;}
        public string UserId { get; set; }

        #endregion
        #region Descriptions
        public string Body { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? Updated { get; set; }
        #endregion

        #region Navigation
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User  { get; set; }

        #endregion

        public TicketComment()
        { 
        }
    }
}