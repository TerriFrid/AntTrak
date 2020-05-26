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
using Microsoft.AspNet.Identity;

namespace AntTrak.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private StringUtilities StringUtilities = new StringUtilities();
        private NotificationHelper notificationHelper = new NotificationHelper();


        // GET: TicketAttachments
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId, FileName")] TicketAttachment ticketAttachment,HttpPostedFileBase Attachment)
        {
            if (ModelState.IsValid)
            {
                if(Attachment != null)
                { 
                    var justFileName = System.IO.Path.GetFileNameWithoutExtension(Attachment.FileName);
                    justFileName = StringUtilities.URLFriendly(justFileName);
                    justFileName = $"{justFileName}--{DateTime.Now.Ticks}";
                    justFileName = $"{justFileName}{System.IO.Path.GetExtension(Attachment.FileName)}";

                    ticketAttachment.FilePath = "/Attachments/" + justFileName;
                    Attachment.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Attachments/"), justFileName));

                }
               
                ticketAttachment.Created = DateTime.Now;
                ticketAttachment.UserId = User.Identity.GetUserId();
                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();

                if (!User.IsInRole("Developer"))
                {
                    var newTicket = db.Tickets.Find(ticketAttachment.TicketId);
                    var newValue = newTicket.Attachments.Count();
                    var oldValue = newValue - 1;

                    var success = notificationHelper.CreateNotification(newTicket, "number of attachments", oldValue.ToString(), newValue.ToString(), true);
                }
                return RedirectToAction("Details", "Tickets", new{Id=ticketAttachment.TicketId });
            }

           
            return RedirectToAction("Details", "Tickets", new{ticketAttachment.TicketId});
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,UserId,FilePath,FileUrl,Description,Created,Updated")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();


            if (!User.IsInRole("Developer"))
            {
                var newTicket = db.Tickets.Find(ticketAttachment.TicketId);
                var newValue = newTicket.Attachments.Count();
                var oldValue = newValue ++;

                var success = notificationHelper.CreateNotification(newTicket, "number of attachments", oldValue.ToString(), newValue.ToString(), true);
            }
            return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });
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
