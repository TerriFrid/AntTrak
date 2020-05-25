using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AntTrak.Models;

namespace AntTrak.ViewModel
{
    public class TicketTable
    {
        public string RequestingAction { get; set; }
        public string RequestingController { get; set; }

        public bool canAccess { get; set; }

        public List<Ticket> RequestedList { get; set; }
    }
}