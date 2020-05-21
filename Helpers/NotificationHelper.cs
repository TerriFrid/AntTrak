using AntTrak.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntTrak.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            //Manage a Developer Assignmentment
            GenerateTicketAssignmentNotifications(oldTicket, newTicket);
            //manage some other general notification
            GenerateTicketChangeNotication(oldTicket, newTicket);
        }


        private void GenerateTicketAssignmentNotifications(Ticket oldTicket, Ticket newTicket)
        {
                bool assigned = oldTicket.DeveloperId == null && newTicket.DeveloperId != null;
            // bool unassigned = oldTicket.DeveloperId !== null && newTicket.DeveloperId != null;
            //  bool reassigned = !assigned && !unassigned && oldTicket.DeveloperId != newTicket.DeveloperId;

            if (assigned)
            {
                var created = DateTime.Now;
                db.TicketNotifications.Add(new TicketNotification
                {
                    Created = created,
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = newTicket.DeveloperId,
                    Subject = "You have been assigned to a Ticket",
                    NotificationBody = $"You have been assigned to Ticket Id: { newTicket.Id } on {created.ToString("MMM, dd, yyyy")}. " 
                   // $"This ticket is on the {newTicket.Project.Name} project."

                }); ;
            }

            db.SaveChanges();
        }

        private void GenerateTicketChangeNotication(Ticket oldTicket, Ticket newTicket)
        {

        }
    }
}