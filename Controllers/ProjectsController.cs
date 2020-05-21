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

        [Authorize(Roles = "Admin")]
        public ActionResult ManageProjectAssignments()
        {                
            var viewData = new List<CustomUserData>();
            var users = db.Users.ToList();
                                      
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
            return View(viewData);           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUsersToProjects (List<string> userIds, List<int> projectIds)
        {
            if (userIds == null || projectIds == null)
            {
                return RedirectToAction("ManageProjectAssignments");
            }

            var pmCount = 0;
            foreach (var userId in userIds)
            {
                if (roleHelper.IsUserInRole(userId, "ProjectManager"))
                {
                    pmCount += 1;
                }
            }
            if (pmCount > 1)
            {
                TempData["UnconfirmedEmailError"] = "You may only select one project manager per assignment.";
                return RedirectToAction("ManageProjectAssignments");
            }
            else 
            {
                foreach (var userId in userIds)
                {
                    foreach(var projectId in projectIds)
                    {
                        projHelper.AddUserToProject(userId, projectId);
                        if (roleHelper.IsUserInRole(userId, "ProjectManager"))
                        {
                            var proj = db.Projects.Find(projectId);
                            proj.ProjectManagerId = userId;
                            db.SaveChanges();
                        }
                    }

               

                }
            }
            return RedirectToAction("ManageProjectAssignments");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUsersFromProject (List<string> userIds, List<int> projectIds)
        {
            if (userIds == null || projectIds == null)
            {
                return RedirectToAction("ManageProjectAssignments");
            }

            foreach (var userId in userIds)
            {
                foreach (var projectId in projectIds)
                {
                    projHelper.RemoveUserFromProject(userId, projectId);
                }

            }

            return RedirectToAction("ManageProjectAssignments");
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult TeamManagement(int id)
        {
            var project = db.Projects.Find(id);
            var projName = project.Name;

            if (User.IsInRole("Admin"))
            {
                ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "FullName", project.ProjectManagerId);
            }
            
            ViewBag.CardTitle = "Team Management: " + projName;
            ViewBag.SubmitterIds = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "FullName");           
            ViewBag.DeveloperIds = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");
           
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            ViewBag.CardTitle = "My Projects";
            ViewBag.MyRole = roleHelper.ListUserRoles(myUserId).FirstOrDefault();

            if (ViewBag.MyRole == "ProjectManager")
            {
                myProjects = db.Projects.Where(p => p.ProjectManagerId == myUserId).ToList();
            }
            else 
            {
                myProjects = projHelper.ListUserProjects(myUserId).ToList();
            }
            
            return View(myProjects);
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
            ApplicationUser projManager = db.Users.Find(project.ProjectManagerId);
            var projManagerName = projManager.Fullname;

            model.Id = project.Id;
            model.Name = project.Name;
            model.Description = project.Description;
            model.ProjectManagerId = project.ProjectManagerId;
            model.ProjectManagerName = projManagerName;
                       
            model.AllTickets = project.Tickets.ToList();
            model.ActiveTickets = project.Tickets.Where(t => t.IsArchived == false).ToList();



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
        public ActionResult Create([Bind(Include = "Id,ProjectManagerId, Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                var pmId = project.ProjectManagerId;
                if (pmId == null)
                {
                    if (User.IsInRole("Admin"))
                    {
                        TempData["InvalidSelection"] = "Please assign a project manager.";
                        var newProject = new Project();
                        var ProjectManagerId = roleHelper.UsersInRole("ProjectManager").ToList();
                        ViewBag.ProjectManagerId = new SelectList(ProjectManagerId, "Id", "FullName");


                        return View(newProject);
                    }
                    else 
                    {
                        pmId = User.Identity.GetUserId();                    
                        
                    }
                }

                project.ProjectManagerId = pmId;
                project.Created = DateTime.Now;
                project.IsArchived = false;
                db.Projects.Add(project);
                db.SaveChanges();

                projHelper.AddUserToProject(pmId, project.Id);
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

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
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
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
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
