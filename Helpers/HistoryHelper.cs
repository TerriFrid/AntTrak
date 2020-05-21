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
            if (OldTicket.Title != NewTicket.Title)
            {
                var newHistoryRecord = new TicketHistory();
               
                newHistoryRecord.ChangedOn = (DateTime)NewTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "Title";
                newHistoryRecord.OldValue = OldTicket.Title;
                newHistoryRecord.NewValue = NewTicket.Title;
                newHistoryRecord.TicketId = NewTicket.Id;
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (OldTicket.Description != NewTicket.Description)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)NewTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "Description";
                newHistoryRecord.OldValue = OldTicket.Description;
                newHistoryRecord.NewValue = NewTicket.Description;
                newHistoryRecord.TicketId = NewTicket.Id;
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (OldTicket.DeveloperId != NewTicket.DeveloperId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)NewTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "DeveloperId";
                newHistoryRecord.OldValue = OldTicket.DeveloperId == null ? "Unassigned" : db.Users.Find(OldTicket.DeveloperId).Fullname; ;
                newHistoryRecord.NewValue = NewTicket.DeveloperId == null ? "Unassigned" : db.Users.Find(NewTicket.DeveloperId).Fullname;
                newHistoryRecord.TicketId = NewTicket.Id;
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (OldTicket.TicketTypeId != NewTicket.TicketTypeId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)NewTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "TicketTypeId";
                newHistoryRecord.OldValue = db.TicketTypes.Find(OldTicket.TicketTypeId).Name;
                newHistoryRecord.NewValue = db.TicketTypes.Find(NewTicket.TicketTypeId).Name;
                newHistoryRecord.TicketId = NewTicket.Id;
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (OldTicket.TicketStatusId != NewTicket.TicketStatusId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)NewTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "TicketStatusId";
                newHistoryRecord.OldValue = db.TicketStatus.Find(OldTicket.TicketStatusId).Name;
                newHistoryRecord.NewValue = db.TicketStatus.Find(NewTicket.TicketStatusId).Name;
                newHistoryRecord.TicketId = NewTicket.Id;
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (OldTicket.TicketPriorityId != NewTicket.TicketPriorityId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)NewTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "TicketPriorityId";
                newHistoryRecord.OldValue = db.TicketPriorities.Find(OldTicket.TicketPriorityId).Name;
                newHistoryRecord.NewValue = db.TicketPriorities.Find(NewTicket.TicketPriorityId).Name;
                newHistoryRecord.TicketId = NewTicket.Id;
                db.TicketHistories.Add(newHistoryRecord);
            }





            db.SaveChanges();
        }

    }
}