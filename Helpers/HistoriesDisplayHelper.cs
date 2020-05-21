using AntTrak.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntTrak.Helpers
{
    public class HistoriesDisplayHelper
    {
        public static string DisplayData(TicketHistory ticketHistory)
        {
            var db = new ApplicationDbContext();
            var data = "";

            switch(ticketHistory.Property)
            {              
                case "DeveloperId":
                    data = db.Users.FirstOrDefault(u => u.Id == ticketHistory.NewValue).Fullname;
                    break;
                case "TicketTypeId":
                    data = db.TicketTypes.Find(ticketHistory.NewValue).Name;
                    break;

                case "StatusId":
                    data = db.TicketStatus.Find(ticketHistory.NewValue).Name;
                    break;
                   
                case "PriorityId":
                    data = db.TicketPriorities.Find(ticketHistory.NewValue).Name;
                    break;
                default:
                    data = ticketHistory.NewValue;
                    break;
            }
            return data;
        }
    }
}