using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntTrak.Models
{
    public class TicketAttachment
    {
    #region Ids
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        #endregion
        #region Descriptions
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        #endregion
        #region Navigation

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

        #endregion

        public TicketAttachment() 
        {         
        }
    }
}