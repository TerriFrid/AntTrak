using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AntTrak.Models;

namespace AntTrak.ViewModel
{
    public class Dashboard
    {
               

        public List<Ticket> AllTickets { get; set; }
        public List<Ticket> AllMyTickets { get; set; }
        public List<Ticket> AllMyProjectsTickets { get; set; }

       
    }
}