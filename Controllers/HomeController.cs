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



namespace AntTrak.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projHelper = new ProjectHelper();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            var tickets = db.Tickets;
            return View(tickets.ToList());
            
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
    }
}