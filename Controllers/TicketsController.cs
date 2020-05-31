using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AntTrak.Helpers;
using AntTrak.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace AntTrak.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projHelper = new ProjectHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();

        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.Developer).Include(t => t.Project).Include(t => t.Submitter).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            
            var projName = ticketHelper.TicketProjectName(ticket.Id);
            ViewBag.CardTitle = projName + ": " + ticket.Title;
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(int? projectId)
        {

            // show only longed on users project
            var myUserId = User.Identity.GetUserId();
            var myProjects = projHelper.ListUserProjects(myUserId);
            ViewBag.CardTitle = "Creat New Ticket ";

            if (projectId == null)
            { 
                ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");                
            }
            else
            {
                var projName = db.Projects.Find(projectId).Name;
                ViewBag.CardTitle += "for " + projName;
            }

            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
          
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            //TLF if we use the same view for edit and create then we would populate this, set it to Unassigned and then disable in view
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");

            var newTicket = new Ticket();
            if(projectId!=null)
            {
                newTicket.ProjectId = (int)projectId;
            }

            return View(newTicket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,TicketPriorityId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.SubmitterId = User.Identity.GetUserId();
                ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(t => t.Name == "Unassigned").Id;              
                ticket.Created = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

           
            if (ticket.ProjectId == 0)
            {
                var myUserId = User.Identity.GetUserId();
                var myProjects = projHelper.ListUserProjects(myUserId);
                ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");
            }

            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");

            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
                      
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize]

        //Custom Action Filter that detects whether or they should be here
        //[SpecialTicketAuth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var bMyTicket = ticketHelper.IsMyTicket(ticket.Id);
                
            if (bMyTicket)
            {
                var projName = ticketHelper.TicketProjectName(ticket.Id);
                var DeveloperId = ticketHelper.AssignableDevelopers(ticket.ProjectId);
                ViewBag.CardTitle = "";
                ViewBag.CardTitle = projName + ": " + ticket.Title;
                
                ViewBag.DeveloperId = new SelectList(DeveloperId, "Id", "FullName", ticket.DeveloperId);
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                return View(ticket);
            }
           else
            {
                TempData["InvalidSelection"] = $"You are not authorized to edit this ticket.";
                return RedirectToAction("Dashboard", "Home");
            }
                                 
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,SubmitterId,DeveloperId,TicketTypeId,TicketStatusId,TicketPriorityId,Title,Description,Created, IsArchived")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //AsNoTracking() to get a Momento Ticket object
                Ticket oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                var ticketStatusName = db.TicketStatus.Find(ticket.TicketStatusId).Name;
                switch (ticketStatusName)
                {
                    case ("Unassigned"):
                        if (ticket.DeveloperId != null)
                        {
                            ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(t => t.Name == "Assigned").Id;
                        }
                        break;
                    case ("Assigned"):
                        if (ticket.DeveloperId == null)
                        {
                            ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(t => t.Name == "Unassigned").Id;
                        }
                        break;
                    case ("Archived"):
                        ticket.IsArchived = true;
                        break;

                }    
               
                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                Ticket newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                //Now I can compare new ticket to the old ticket for changes that need to be recorded in the Ticket History table
                // call History Helper
                historyHelper.ManageHistoryRecordCreation(oldTicket, newTicket);

                if(!User.IsInRole("Developer") )
                { 
                    notificationHelper.ManageNotifications(oldTicket, newTicket);
                }
                return RedirectToAction("Details", "Tickets", new { ticket.Id });
               
            }

            //TLF this needs to match Edit GET
            var bMyTicket = ticketHelper.IsMyTicket(ticket.Id);

            if (bMyTicket)
            {
                var projName = ticketHelper.TicketProjectName(ticket.Id);
                var DeveloperId = ticketHelper.AssignableDevelopers(ticket.ProjectId);
                ViewBag.CardTitle = "";
                ViewBag.CardTitle = projName + ": " + ticket.Title;

                ViewBag.DeveloperId = new SelectList(DeveloperId, "Id", "FullName", ticket.DeveloperId);
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                return View(ticket);
            }
            else
            {
                TempData["InvalidSelection"] = $"You are not authorized to edit this ticket.";
                return RedirectToAction("Dashboard", "Home");
            }
            return View(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
