using AntTrak.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntTrak.Helpers
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void ManageHistoryRecordCreation(Ticket OldTicket, Ticket NewTicket)
        {
            if(OldTicket.Title != NewTicket.Title)
            {
               
                    var newHistoryRecord = new TicketHistory();
                    newHistoryRecord.ChangedOn = (DateTime)NewTicket.Updated;
                    newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                    newHistoryRecord.Property = "Title";
                    newHistoryRecord.OldValue = OldTicket.Title;
                    newHistoryRecord.NewValue = NewTicket.Title;
                    newHistoryRecord.Id = NewTicket.Id;
                    db.TicketHistories.Add(newHistoryRecord);
             
            }

            db.SaveChanges();
        }

    }
}