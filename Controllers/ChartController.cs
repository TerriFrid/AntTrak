using AntTrak.Models;
using AntTrak.ViewModel;
using AntTrak.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AntTrak.Controllers
{
    [Authorize]
    public class ChartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper ticketHelper = new TicketHelper();
        //GET: Chart
        public JsonResult GetAllTicketPriorityChartData()
        {
            var pieChartVM = new PieCharts();
            var priorities = db.TicketPriorities.ToList();
            var myTickets = ticketHelper.ListMyTickets();
           
            foreach (var priority in priorities)
            {
                pieChartVM.Labels.Add(priority.Name);
                pieChartVM.Colors.Add(priority.Color);
                pieChartVM.Values.Add(myTickets.Where(t => t.TicketPriorityId == priority.Id).Count());
            }
            return Json(pieChartVM);
        }
        public JsonResult GetActiveTicketPriorityChartData()
        {
            var pieChartVM = new PieCharts();
            var priorities = db.TicketPriorities.ToList();
            var ticketStatusUnassigned = db.TicketStatus.FirstOrDefault(s => s.Name == "Unassigned").Id;
            var ticketStatusAssigned = db.TicketStatus.FirstOrDefault(s => s.Name == "Assigned").Id;
            var myTickets = ticketHelper.ListMyTickets();
            //Need the Where syntax Where TicketStatusId IN(ticketStatusUnassigned, ticketStatusAssigned)
            myTickets  = myTickets.Where(t => t.TicketStatusId == ticketStatusUnassigned || t.TicketStatusId == ticketStatusAssigned).ToList();    

            foreach (var priority in priorities)
            {
                pieChartVM.Labels.Add(priority.Name);
                pieChartVM.Colors.Add(priority.Color);
                pieChartVM.Values.Add(myTickets.Where(t => t.TicketPriorityId == priority.Id).Count());

            }
            return Json(pieChartVM);
        }

        public JsonResult GetAllTicketStatusChartData()
        {
            var pieChartVM = new PieCharts();
            var statuses = db.TicketStatus.ToList();
            var myTickets = ticketHelper.ListMyChartTickets();

            foreach (var status in statuses)
            {
                pieChartVM.Labels.Add(status.Name);
                pieChartVM.Colors.Add(status.Color);
                pieChartVM.Values.Add(myTickets.Where(t => t.TicketStatusId == status.Id).Count());

            }
            return Json(pieChartVM);
        }

        public JsonResult GetAllTicketTypesChartData()
        {
            var pieChartVM = new PieCharts();
            var types = db.TicketTypes.ToList();
            var myTickets = ticketHelper.ListMyTickets();

            foreach (var type in types)
            {
                pieChartVM.Labels.Add(type.Name);
                pieChartVM.Colors.Add(type.Color);
                pieChartVM.Values.Add(myTickets.Where(t => t.TicketTypeId == type.Id).Count());

            }
            return Json(pieChartVM);
        }

        public JsonResult GetActiveTicketTypeChartData()
        {
            var pieChartVM = new PieCharts();
            var types = db.TicketTypes.ToList();
            var ticketStatusUnassigned = db.TicketStatus.FirstOrDefault(s => s.Name == "Unassigned").Id;
            var ticketStatusAssigned = db.TicketStatus.FirstOrDefault(s => s.Name == "Assigned").Id;
            var myTickets = ticketHelper.ListMyTickets();
            myTickets = myTickets.Where(t => t.TicketStatusId == ticketStatusUnassigned).ToList();

            foreach (var type in types)
            {
                pieChartVM.Labels.Add(type.Name);
                pieChartVM.Colors.Add(type.Color);
                pieChartVM.Values.Add(myTickets.Where(t => t.TicketTypeId == type.Id).Count());

            }
            return Json(pieChartVM);
        }
    }
}