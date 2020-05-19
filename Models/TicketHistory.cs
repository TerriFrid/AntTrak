using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AntTrak.Models
{
    public class TicketHistory
    {
        #region Ids
        public int Id { get; set; }
        public int TicketId { get; set; }
        public String UserId { get; set; }       
        
        #endregion
        #region Descriptions
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime ChangedOn { get; set; }

        #endregion

        #region navigation
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
        #endregion

        public TicketHistory() 
        { 
        }

    }
}