using AntTrak.Models;
using AntTrak.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mail;
using System.Web.Mvc;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using AntTrak.ViewModel;

namespace AntTrak.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projHelper = new ProjectHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            var model = new Dashboard();

            model.AllMyTickets = ticketHelper.ListMyTickets();
            model.AllTickets = db.Tickets.ToList();
            if (User.IsInRole("Developer")) 
            {  
                var currentUserId = User.Identity.GetUserId();
                model.AllMyProjectsTickets = ticketHelper.ListMyProjectsTickets(currentUserId);
                ViewBag.CardTitleDev = "My Projects' Tickets";

            }
            ViewBag.CardTitle = "All Tickets";
            ViewBag.CardTitle2 = "My Tickets";
            return View(model);
            
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var from = $"{model.FromName}<{WebConfigurationManager.AppSettings["emailfrom"]}>";
                    var email = new System.Net.Mail.MailMessage(from, WebConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = model.Subject,
                        Body = model.Body,
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return RedirectToAction("Dashboard", "Home");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            //TLF is this correct?
            return View(model);
        }
        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            return View(model);
        }

        public PartialViewResult _SideNav()
        {
            var model = db.Users.Find(User.Identity.GetUserId());

            return PartialView(model);
        }
    }
}