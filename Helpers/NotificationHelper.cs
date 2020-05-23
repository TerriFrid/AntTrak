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
            var Assigned = oldTicket.DeveloperId == null && newTicket.DeveloperId != null;
            var Unassigned = oldTicket.DeveloperId != null && newTicket.DeveloperId == null;
            var Reassigned = !Assigned && !Unassigned && oldTicket.DeveloperId != newTicket.DeveloperId;


                //assigned
            if (Assigned)
            {
                GenerateNotification(newTicket, "assigned to");
                db.SaveChanges();
            }

            // unassigned      
            if (Unassigned)
            {         
                GenerateNotification(oldTicket, "unassigned from");
                db.SaveChanges();
            }

            //reassigned
            if (Reassigned)
            {
                GenerateNotification(oldTicket, "unassigned from");
                GenerateNotification(newTicket, "assigned to");
                db.SaveChanges();
            }

        }

        private void GenerateNotification(Ticket ticket, string action)
        {
            db.TicketNotifications.Add(new TicketNotification
            {
                Created = DateTime.Now,
                TicketId = ticket.Id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = ticket.DeveloperId,
                Subject = $"You have been {action} a Ticket",
                NotificationBody = $"You have been {action} Ticket Id: { ticket.Title } on {DateTime.Now}. This ticket is on the {ticket.Project.Name} project."

            });
        }
        private void GenerateTicketChangeNotication(Ticket oldTicket, Ticket newTicket)
        {
            var isChanged = false;
            if (CreateNotification(newTicket, "title", oldTicket.Title, newTicket.Title))
            {
                isChanged = true;
            }
            if (CreateNotification(newTicket, "description", oldTicket.Description, newTicket.Description))
            {
                isChanged = true;
            }

            if (CreateNotification(newTicket, "ticket type", oldTicket.TicketType.Name, newTicket.TicketType.Name))
            {
                isChanged = true;
            }

            if (CreateNotification(newTicket, "status", oldTicket.TicketStatus.Name, newTicket.TicketStatus.Name))
            {
                isChanged = true;
            }

            if (CreateNotification(newTicket, "priority", oldTicket.TicketPriority.Name, newTicket.TicketPriority.Name))
            {
                isChanged = true;
            }
                       
            if (isChanged)
            {
                db.SaveChanges();
            }
        }
        

        public bool CreateNotification(Ticket newTicket, string property, string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                db.TicketNotifications.Add(new TicketNotification
                {
                    Created = DateTime.Now,
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = newTicket.DeveloperId,
                    Subject = "One of your assigned tickets has changed",
                    NotificationBody = $"Project: {newTicket.Project.Name} Ticket: { newTicket.Title} has been updated. The {property} has changed from {oldValue} to {newValue}."
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
