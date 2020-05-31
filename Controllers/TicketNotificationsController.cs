using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AntTrak.Models;
using AntTrak.Helpers;

namespace AntTrak.Controllers
{
    [Authorize]
    public class TicketNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //private MessageHelper messageHelper = new MessageHelper();
        // GET: TicketNotifications

        [Authorize(Roles = "Developer")]
        public ActionResult Index()
        {
            ViewBag.CardTitle = "Notifications";
            var ticketNotifications = MessageHelper.GetAllNotifications();
            return View(ticketNotifications.ToList());
        }

        [Authorize(Roles = "Developer")]
        public ActionResult DismissFromDashboard(int id)
        {
            var notification = db.TicketNotifications.Find(id);
            notification.IsRead = true;
            db.SaveChanges();
           
            return RedirectToAction("Dashboard", "Home");
        }

        [Authorize(Roles = "Developer")]
        public ActionResult DismissFromIndex(int id)
        {
            var notification = db.TicketNotifications.Find(id);
            notification.IsRead = true;
            db.SaveChanges();

            return RedirectToAction("Index", "TicketNotifications");
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
