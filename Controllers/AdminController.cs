﻿using AntTrak.Helpers;
using AntTrak.Models;
using AntTrak.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AntTrak.Controllers
{

    [Authorize(Roles ="Admin")]    
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        // GET: Admin
        public ActionResult ManageRoles()
        {
            var roleManagementVMs = new List<RoleManagement>();



            //var viewData = new List<CustomUserData>();
            var allUsers = db.Users.ToList();
            //foreach (var user in allUsers)
            //{
            //    roleManagementVMs.Add(new RoleManagement
            //    {
            //        customUser
            //        {
            //            ful
            //        }
            //        Roles = new SelectList(db.Roles, "Id", "Name", user.Roles.FirstOrDefault().RoleId)
            //    });

                //viewData.Add(new CustomUserData
                //{
                //    FirstName = user.FirstName,
                //    LastName = user.LastName,
                //    Email = user.Email,
                //    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault() ?? "Unassigned"
                //});
          // }

        // ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

        // ViewBag.CardTitle = "Manage User Roles";           

       // View(roleManagementVMs);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            if (userIds != null)
            {
                foreach(var userId in userIds)
                {                    
                    var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

                    if(!string.IsNullOrEmpty(userRole))
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

            return RedirectToAction("Dashboard", "Home");        }
        

    }
}