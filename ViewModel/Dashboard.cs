using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AntTrak.Models;

namespace AntTrak.ViewModel
{
    public class Dashboard
    {
       
        public int HighPriorityUnassigned { get; set; }
        public int HighPriorityAssigned { get; set; }
        public int MediumPriorityUnassigned { get; set; }
        public int MediumPriorityAssigned { get; set; }

        public int UnAssignedMoreThan2Days { get; set; }
        public int AssignedMoreThan7Days { get; set; }

        public int OpenDefects { get; set; }
        public int OpenDisplay { get; set; }
        public int OpenFunctionality { get; set; }
        public int OpenNewFeature { get; set; }
   


        public List<Ticket> AllTickets { get; set; }
        public List<Ticket> AllMyTickets { get; set; }
        public List<Ticket> AllMyProjectsTickets { get; set; }

       public TicketTable TicketTableVM { get; set; }

        public Dashboard()
        {
            TicketTableVM = new TicketTable();
        }
    }
}