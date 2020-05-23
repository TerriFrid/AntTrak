using AntTrak.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace AntTrak.Helpers
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void ManageHistoryRecordCreation(Ticket OldTicket, Ticket NewTicket)
        {
            var isCreated = false;

            if (CreateHistoryRecord(NewTicket, "Title", OldTicket.Title, NewTicket.Title))
            {
                isCreated = true;
            }

            if (CreateHistoryRecord(NewTicket, "Description", OldTicket.Description, NewTicket.Description))
            {
                isCreated = true;
            }

            if (CreateHistoryRecord(NewTicket, "DeveloperId", OldTicket.DeveloperId, NewTicket.DeveloperId))
            {
                isCreated = true;
            }
            if (CreateHistoryRecord(NewTicket, "TicketTypeId", OldTicket.TicketType.Name, NewTicket.TicketType.Name))
            {
                isCreated = true;
            }

            if (CreateHistoryRecord(NewTicket, "TicketStatusId", OldTicket.TicketStatus.Name, NewTicket.TicketStatus.Name))
            {
                isCreated = true;
            }

            if (CreateHistoryRecord(NewTicket, "TicketPriorityId", OldTicket.TicketPriority.Name, NewTicket.TicketPriority.Name))
            {
                isCreated = true;
            }

            if (isCreated)
            {
                db.SaveChanges();
            }            
        }

        private bool CreateHistoryRecord (Ticket NewTicket, string property, string oldValue, string newValue)
        {
           if(oldValue != newValue)
            {                  
                db.TicketHistories.Add (new TicketHistory
                {
                    ChangedOn = (DateTime)NewTicket.Updated,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    Property = property,
                    OldValue = property != "DeveloperId" ? oldValue : oldValue == null ? "unassigned" : db.Users.Find(oldValue).Fullname,
                    NewValue = property != "DeveloperId" ? newValue : newValue == null ? "unassigned" : db.Users.Find(newValue).Fullname,
                    TicketId = NewTicket.Id
                });                
                return true;
            }
           else
            {
                return false;
            }
        }
    }
}