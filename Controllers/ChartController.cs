using AntTrak.Models;
using AntTrak.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AntTrak.Controllers
{
    
    public class ChartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //GET: Chart
        public JsonResult GetAllTicketPriorityChartData()
        {
            var pieChartVM = new PieCharts();
            var priorities = db.TicketPriorities.ToList();
            
           
            foreach (var priority in priorities)
            {
                pieChartVM.Labels.Add(priority.Name);
                pieChartVM.Colors.Add(priority.Color);
                pieChartVM.Values.Add(db.Tickets.Where(t => t.TicketPriorityId == priority.Id).Count());

            }
            return Json(pieChartVM);
        }
        public JsonResult GetActiveTicketPriorityChartData()
        {
            var pieChartVM = new PieCharts();
            var priorities = db.TicketPriorities.ToList();
            var ticketStatusUnassigned = db.TicketStatus.Where(s => s.Name == "Unassigned").FirstOrDefault().Id;
            var ticketStatusAssigned = db.TicketStatus.Where(s => s.Name == "Assigned");

            //Need the Where syntax Where TicketStatusId IN(ticketStatusUnassigned, ticketStatusAssigned)
            var tickets = db.Tickets.Where(t => t.TicketStatusId == ticketStatusUnassigned).ToList();    

            foreach (var priority in priorities)
            {
                pieChartVM.Labels.Add(priority.Name);
                pieChartVM.Colors.Add(priority.Color);
                pieChartVM.Values.Add(db.Tickets.Where(t => t.TicketPriorityId == priority.Id).Count());

            }
            return Json(pieChartVM);
        }
    }
}