using AntTrak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntTrak.Helpers
{
    public class AssignmentHelper
    {
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projHelper = new ProjectHelper();

        public ICollection<ApplicationUser> UsersOnProjectInRole(int projectId, string roleName)
        {
            var users = new List<ApplicationUser>();
            var projUsers = projHelper.ListUsersOnProject(projectId);

            foreach(var user in projUsers)
            {
                if(roleHelper.ListUserRoles(user.Id).FirstOrDefault()== roleName)
                {
                    users.Add(user);
                }
            }

            return users;
        }

    }
}