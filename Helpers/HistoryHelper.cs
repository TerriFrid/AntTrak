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
                newHistoryRecord.Id = NewTicket.Id;
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
                newHistoryRecord.Id = NewTicket.Id;
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (OldTicket.DeveloperId != NewTicket.DeveloperId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)NewTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "DeveloperId";
                newHistoryRecord.OldValue = OldTicket.DeveloperId;
                newHistoryRecord.NewValue = NewTicket.DeveloperId;
                newHistoryRecord.Id = NewTicket.Id;
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (OldTicket.TicketTypeId != NewTicket.TicketPriorityId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)NewTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "TicketPriorityId";
                newHistoryRecord.OldValue = OldTicket.TicketTypeId.ToString();
                newHistoryRecord.NewValue = NewTicket.TicketTypeId.ToString();
                newHistoryRecord.Id = NewTicket.Id;
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (OldTicket.TicketStatusId != NewTicket.TicketStatusId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)NewTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "TicketStatusId";
                newHistoryRecord.OldValue = OldTicket.TicketStatusId.ToString();
                newHistoryRecord.NewValue = NewTicket.TicketStatusId.ToString();
                newHistoryRecord.Id = NewTicket.Id;
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (OldTicket.TicketPriorityId != NewTicket.TicketPriorityId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)NewTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "TicketPriorityId";
                newHistoryRecord.OldValue = OldTicket.TicketPriorityId.ToString();
                newHistoryRecord.NewValue = NewTicket.TicketPriorityId.ToString();
                newHistoryRecord.Id = NewTicket.Id;
                db.TicketHistories.Add(newHistoryRecord);
            }





            db.SaveChanges();
        }

    }
}