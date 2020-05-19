using AntTrak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace AntTrak.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projHelper = new ProjectHelper();

        public ICollection<ApplicationUser> AssignableDevelopers(int projectId)
        {          
            var developers = roleHelper.UsersInRole("Developer").ToList();
            //var onProject = projHelper.UserOnProject(projectId);
            var assignableDevelopers = new List<ApplicationUser>();

            //return developers.Intersect(onProject).ToList();
            foreach(var dev in developers)
            {
                if (projHelper.IsUserOnProject(dev.Id, projectId))
                {
                    assignableDevelopers.Add(dev);
                }
            }

            return assignableDevelopers;

        }
                
        public List<Ticket> ListMyTickets()
        {
            var myTickets = new List<Ticket>();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                case "DemoAdmin":
                    myTickets.AddRange(db.Tickets);
                    break;
                case "ProjectManager":
                case "DemoPM":
                    myTickets.AddRange(db.Projects.Where(p => p.ProjectManagerId == userId).SelectMany(p => p.Tickets));
                    break;
                case "Developer":
                case "DemoDeveloper":
                    myTickets.AddRange(db.Tickets.Where(t => t.IsArchived == false).Where(t => t.DeveloperId == userId));
                    break;
                case "Submitter":
                case "DemoSubmitter":
                    myTickets.AddRange(db.Tickets.Where(t => t.IsArchived == false).Where(t => t.SubmitterId == userId));
                    break;
            }
            return myTickets;

        }
        public List<Ticket> ListMyProjectsTickets(string userId)
        {
            var myProjects = projHelper.ListProjectsForUser(userId);
            var myProjectTickets = myProjects.SelectMany(p => p.Tickets).ToList();
            return myProjectTickets;
        }
        public bool IsMyTicket(int TicketId)
        {
            var ticket = db.Tickets.Find(TicketId);
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var role = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var flag = false;

            switch (role)
                {
                    case "Submitter":
                        if (userId == ticket.SubmitterId)
                        {
                            flag = true;
                        }
                        break;
                    case "Developer":
                        if (userId == ticket.DeveloperId)
                        {
                            flag = true;
                    }
                        break;
                    case "ProjectManager":
                        var testTicket = db.Projects.Where(p => p.ProjectManagerId == userId).SelectMany(p => p.Tickets).Where(t => t.Id == ticket.Id);
                        if (testTicket != null)
                        {
                            flag = true;
                        }
                        break;
                case "Admin":
                        //This is admin and can edit anything
                        flag = true;
                        break;
                }

            return flag;
        }

        public string TicketProjectName (int TicketId)
        {
            var projName = db.Tickets.Find(TicketId).Project.Name;
            return projName;
        }

    }
}