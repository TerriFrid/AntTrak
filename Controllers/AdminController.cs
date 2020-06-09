using AntTrak.Helpers;
using AntTrak.Models;
using AntTrak.ViewModel;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TFridBlog.Helpers;

namespace AntTrak.Controllers
{

    [Authorize(Roles ="Admin")]    
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private ProjectHelper projHelper = new ProjectHelper();

        // GET: Admin
        public ActionResult ManageRoles()
        {
            //var manageRolesVm = new List<RoleManagement>();
            //var allUsers = db.Users.ToList();

            //foreach (var user in allUsers)
            //{
            //    manageRolesVm.Add(new RoleManagement
            //    {
            //        CurrentUser = user,
            //        Roles = new SelectList(db.Roles, "Id", "Name", user.Roles.FirstOrDefault().RoleId)
            //    });
            //}
            //   
            //return View(manageRolesVm);

            ViewBag.CardTitle = "Manage User Roles";
            var viewData = new List<CustomUserData>();
            var users = db.Users.ToList().OrderBy(u=>u.Fullname);
            foreach (var user in users)
            {
                viewData.Add(new CustomUserData
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault() ?? "Unassigned"
                });
            }

            ViewBag.UserIds = new MultiSelectList(db.Users.ToList().OrderBy(u => u.Fullname), "Id", "Fullname");

            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name");

            return View(viewData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            if (userIds != null)
            {
                foreach (var userId in userIds)
                {
                    var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

                    if (!string.IsNullOrEmpty(userRole))
                    {
                        roleHelper.RemoveUserFromRole(userId, userRole);
                    }

                    if (!string.IsNullOrEmpty(roleName))
                    {
                        roleHelper.AddUserToRole(userId, roleName);
                        // TLF if the role is admin the user needs to be added to all existing projects.
                    }
                }
            }
            return RedirectToAction("ManageRoles");
        }

        public ActionResult UserProfileIndex()
        {
            ViewBag.CardTitle = "User Profiles";
            var userProfilesVM = new List<UserProfile>();
            var users = db.Users.ToList().OrderBy(u => u.LastName);

            foreach (var user in users)
            {
                userProfilesVM.Add(new UserProfile
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = roleHelper.ListUserRoles(user.Id).FirstOrDefault(),
                    Email = user.Email,
                    AvatarUrl = user.AvatarPath
                });

            }
            return View(userProfilesVM);
        }

        // GET: UserProfiles/Details/
        [Authorize]
        public ActionResult UserProfileDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = new UserProfile();
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            userProfile.FirstName = user.FirstName;
            userProfile.LastName = user.LastName;
            userProfile.Email = user.Email;
            userProfile.AvatarUrl = user.AvatarPath;
            userProfile.Role = roleHelper.ListUserRoles(user.Id).FirstOrDefault();
            userProfile.MyProjects = projHelper.ListProjectsForUser(user.Id).ToList();
            ViewBag.CardTitle = "User Profile";

            return View(userProfile);
        }


        [AllowAnonymous]
        public ActionResult UserProfileEdit()
        {
                        
            ViewBag.CardTitle = "Edit Your User Profile";
            var currentUser = db.Users.Find(User.Identity.GetUserId());
            var user = new UserProfile();

            user.Id = currentUser.Id;
            user.FirstName = currentUser.FirstName;
            user.LastName = currentUser.LastName;
            user.Email = currentUser.Email;
            user.AvatarUrl = currentUser.AvatarPath;

            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfileEdit(UserProfile model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var justFileName = System.IO.Path.GetFileNameWithoutExtension(image.FileName);
                    justFileName = StringUtilities.URLFriendly(justFileName);
                    justFileName = $"{justFileName}--{DateTime.Now.Ticks}";
                    justFileName = $"{justFileName}{System.IO.Path.GetExtension(image.FileName)}";

                    model.AvatarUrl = "/Avatars/" + justFileName;
                    image.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Avatars/"), justFileName));
                }


                var currentUser = db.Users.Find(User.Identity.GetUserId());
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;
                currentUser.UserName = model.Email;
                currentUser.Email = model.Email;
                currentUser.AvatarPath = model.AvatarUrl;

                db.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            return View();
        }

    }
}