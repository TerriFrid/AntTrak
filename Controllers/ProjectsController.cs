using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using AntTrak.Helpers;
using AntTrak.Models;
using AntTrak.ViewModel;
using Microsoft.AspNet.Identity;

namespace AntTrak.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projHelper = new ProjectHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private AssignmentHelper assignHelper = new AssignmentHelper();
        private TicketHelper ticketHelper = new TicketHelper();

        [Authorize(Roles = "Admin")]
        public ActionResult ManageUserProjectAssignments()
        {                
            var viewData = new List<CustomUserData>();
            var users = roleHelper.UsersNotInRole("ProjectManager").ToList().OrderBy(u=>u.Fullname);
                                      
            //Load Multi Select of Users
            ViewBag.UserIds = new MultiSelectList(users, "Id", "FullName");
           
            //Load Multi Select of Projects
            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name");
          
            //Load View Model

            foreach (var user in users)
            {                  
                viewData.Add(new CustomUserData
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    ProjectNames = projHelper.ListUserProjects(user.Id).Select(p => p.Name).ToList()
                });
               
            }
            ViewBag.CardTitle = "Manage User Project Assignment";
            return View(viewData);           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult ManageUserProjectAssignments(List<string> userIds, List<int> projectIds, bool addUser)
        {
            if (userIds == null || projectIds == null)
            {
                TempData["InvalidSelection"] = "Please select both a user and a project";
               
            }
            else
            {
                foreach (var userId in userIds)
                {
                    foreach (var projectId in projectIds)
                    {
                        if(addUser)
                        { 
                            projHelper.AddUserToProject(userId, projectId);
                        }
                        else
                        {
                            projHelper.RemoveUserFromProject(userId, projectId);
                        }
                    }
                }
            }
            return RedirectToAction("ManageUserProjectAssignments");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ManageProjectUserAssignments()
        {
            //var viewData = new List<CustomUserData>();
            var users = roleHelper.UsersNotInRole("ProjectManager").ToList().OrderBy(u => u.Fullname);
            var projects = db.Projects.Where(p => p.IsArchived == false).OrderBy(p => p.Name).ToList();

            var projectVM = new List <ProjectDetails>();
            foreach(var project in projects)
            {
                projectVM.Add(new ProjectDetails
                {
                    Id = project.Id,
                    Name = project.Name,
                    ProjectManagerId = project.ProjectManagerId,
                    ProjectManagerName = project.ProjectManagerId == null ? "Unassigned" : db.Users.Find(project.ProjectManagerId).Fullname,
                    ProjectMembers = projHelper.ProjectMembers(project.Id).ToList()
                }); 
            }

            //Load Multi Select of Users
            ViewBag.UserIds = new MultiSelectList(users, "Id", "FullName");

            //Load Multi Select of Projects
            ViewBag.ProjectIds = new MultiSelectList(projects, "Id", "Name");

            
            ViewBag.CardTitle = "Manage Project User Assignment";
            return View(projectVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult ManageProjectUserAssignments(List<string> userIds, List<int> projectIds, bool addUser)
        {
            if (userIds == null || projectIds == null)
            {
                TempData["InvalidSelection"] = "Please select both a user and a project";

            }
            else
            {
                foreach (var userId in userIds)
                {
                    foreach (var projectId in projectIds)
                    {
                        if (addUser)
                        {
                            projHelper.AddUserToProject(userId, projectId);
                        }
                        else
                        {
                            projHelper.RemoveUserFromProject(userId, projectId);
                        }
                    }
                }
            }
            return RedirectToAction("ManageProjectUserAssignments");
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult TeamManagement(int id)
        {
            var project = db.Projects.Find(id);
            var projName = project.Name;
            var ProjectManagerId = roleHelper.UsersInRole("ProjectManager").ToList();

            if (User.IsInRole("Admin"))
            {
                ViewBag.ProjectManagerId = new SelectList(ProjectManagerId, "Id", "FullName", project.ProjectManagerId);
               // ViewBag.ProjectManagerId = new SelectList(, "Id", "FullName", project.ProjectManagerId);
            }
            
            ViewBag.CardTitle = "Team Management: " + projName;
            ViewBag.SubmitterIds = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "FullName");           
            ViewBag.DeveloperIds = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");
           
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
       
        public ActionResult TeamManagement(int id, string projectManagerId, List<string> SubmitterIds, List<string> DeveloperIds, bool addMember)
        {
            var project = db.Projects.Find(id);

            if (SubmitterIds == null && DeveloperIds == null)
            {
                TempData["InvalidSelection"] = "Please select a user";
            }
            else
            {
                if (SubmitterIds != null)
                {
                    foreach (var submitterId in SubmitterIds)
                    {
                        if (projHelper.IsUserOnProject(submitterId, project.Id) && !addMember)
                        {
                            projHelper.RemoveUserFromProject(submitterId, project.Id);
                        }
                        else if (!projHelper.IsUserOnProject(submitterId, project.Id) && addMember)
                        {
                            projHelper.AddUserToProject(submitterId, project.Id);
                        }
                    }
                }

                if (DeveloperIds != null)
                {
                    foreach (var developerId in DeveloperIds)
                    {
                        if (projHelper.IsUserOnProject(developerId, project.Id) && !addMember)
                        {
                            projHelper.RemoveUserFromProject(developerId, project.Id);
                        }
                        else if (!projHelper.IsUserOnProject(developerId, project.Id) && addMember)
                        {
                            projHelper.AddUserToProject(developerId, project.Id);
                        }
                    }
                }   
            }

            project.ProjectManagerId = projectManagerId;
            db.SaveChanges();

            return RedirectToAction("TeamManagement", new {project.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult AssignMembers(int id, string projectManagerId, List<string> SubmitterIds, List<string> DeveloperIds)
        {
            var project = db.Projects.Find(id);
           
            if (SubmitterIds == null && DeveloperIds == null)
            {
                TempData["InvalidSelection"] = "Please select a user to add to the project.";                    
            }
            else 
            {
                if (SubmitterIds != null)
                { 
                    foreach (var user in assignHelper.UsersOnProjectInRole(project.Id, "Submitter"))
                    {
                        projHelper.RemoveUserFromProject(user.Id, project.Id);
                    }
                    foreach(var submitterId in SubmitterIds)
                    {
                        projHelper.AddUserToProject(submitterId, project.Id);
                    }
                }

                if (DeveloperIds != null)
                {
                    foreach (var user in assignHelper.UsersOnProjectInRole(project.Id, "Developer"))
                    {
                        projHelper.RemoveUserFromProject(user.Id, project.Id);
                    }
                    foreach (var developerId in DeveloperIds)
                    {
                        projHelper.AddUserToProject(developerId, project.Id);
                    }
                }
            }

            project.ProjectManagerId = projectManagerId;
            db.SaveChanges();

            return View(project);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult RemoveMembers(int id, List<string> SubmitterIds, List<string> DeveloperIds)
        {
            var project = db.Projects.Find(id);
            if (SubmitterIds == null && DeveloperIds == null)
            {
                TempData["InvalidSelection"] = "Please select a user to remove from project.";
            }
            
            //    else
            //    {
            //        foreach (var memberId in notMemberIds)
            //        {
            //        projHelper.RemoveUserFromProject(memberId, id);
            //    }
                
            //}
            
                   

            return View(project);


        }
        
        // GET: Projects
        public ActionResult Index()
        {
            var myUserId = User.Identity.GetUserId();
            var myProjects = new List<Project>();
            var myProjectsVMs = new List<ProjectVM>();
            ViewBag.CardTitle = "My Projects";
           

            if (roleHelper.IsUserInRole(myUserId, "ProjectManager"))
            {
                myProjects = db.Projects.Where(p => p.ProjectManagerId == myUserId).ToList();
            }
            else 
            {
                myProjects = projHelper.ListUserProjects(myUserId).ToList();
            }

            foreach(var project in myProjects)
            {
                myProjectsVMs.Add(new ProjectVM
                {
                    id = project.Id,
                    name = project.Name,
                    pmId = project.ProjectManagerId,
                    pmName = project.ProjectManagerId != null ? db.Users.Find(project.ProjectManagerId).Fullname : "Unassigned",
                    description = project.Description
                });
            }
            
            return View(myProjectsVMs);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardTitle = project.Name;
            var model = new ProjectDetails();
            var projManagerName = "Unassigned";
            if (project.ProjectManagerId!=null)
            { 
                ApplicationUser projManager = db.Users.Find(project.ProjectManagerId);
                projManagerName = projManager.Fullname;
            }
            model.Id = project.Id;
            model.Name = project.Name;
            model.Description = project.Description;
            model.ProjectManagerId = project.ProjectManagerId;
            model.ProjectManagerName = projManagerName;
                       
            model.ClosedTickets = project.Tickets.Where(t => t.IsArchived == true).ToList();
            model.ActiveTickets = project.Tickets.Where(t => t.IsArchived == false).ToList();


            //var usersOnProject = projHelper.ProjectMembers(project.Id).ToList();
            //foreach (var user in usersOnProject)
            //{
            //    model.EnhancedProjectMembers.Add(new UserProfile
            //    {
            //        Id = user.Id,
            //        FirstName = user.FirstName,
            //        LastName = user.LastName,
            //        AvatarUrl = user.AvatarPath,                    
            //       // Role = roleHelper.ListUserRoles(user.Id).FirstOrDefault(),
            //        nbrOpenTickets = ticketHelper.ListMyOpenProjectsTickets().Count
            //    });
            //}
                model.ProjectMembers = projHelper.ProjectMembers(project.Id).ToList();
            
            return View(model);
        }

        // GET: Projects/Create
        [Authorize (Roles="Admin, ProjectManager")]
        public ActionResult Create()
        {
            var newProject = new Project();
            var ProjectManagerId = roleHelper.UsersInRole("ProjectManager").ToList();
            ViewBag.ProjectManagerId = new SelectList(ProjectManagerId, "Id", "FullName");

            
            return View(newProject);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create([Bind(Include = "Id,ProjectManagerId, Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                    project.Created = DateTime.Now;
                    project.IsArchived = false;
                    db.Projects.Add(project);
                    db.SaveChanges();

                    //projHelper.AddUserToProject(pmId, project.Id);
                    //need to add all the admins to the project
                    var Admins = roleHelper.UsersInRole("Admin").ToList();
                    foreach (var admin in Admins)
                    {
                        projHelper.AddUserToProject(admin.Id, project.Id);
                    };
                return RedirectToAction("Index");
            }  
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();
            var ProjectManagerId = roleHelper.UsersInRole("ProjectManager").ToList();
            ViewBag.ProjectManagerId = new SelectList(ProjectManagerId, "Id", "FullName", project.ProjectManagerId);           
            ViewBag.CardTitle = project.Name;
            
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ProjectManagerId,Created,Updated,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                project.Updated = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Details", "Projects", new {project.Id } );
            }
            var ProjectManagerId = roleHelper.UsersInRole("ProjectManager").ToList();
            ViewBag.ProjectManagerId = new SelectList(ProjectManagerId, "Id", "FullName", project.ProjectManagerId);
            ViewBag.CardTitle = project.Name;

            return View(project);
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
