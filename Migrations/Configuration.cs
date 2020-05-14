namespace AntTrak.Migrations
{
    using AntTrak.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AntTrak.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AntTrak.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            #region Create Roles
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "SuperAdmin"))
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
            }
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });

                if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

                #endregion

                #region Add Users and Assign Roles

                var userStore = new UserStore<Models.ApplicationUser>(context);
                var userManager = new UserManager<Models.ApplicationUser>(userStore);


                if (!context.Users.Any(u => u.Email == "terriafsusa@gmail.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "terriafsusa@gmail.com",
                        Email = "terriafsusa@gmail.com",
                        FirstName = "Terri",
                        LastName = "Frid"
                    };
                    userManager.Create(user, "Test123!");
                    userManager.AddToRoles(user.Id, "Admin");
                }

                if (!context.Users.Any(u => u.Email == "JasonTwichell@coderfoundry.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "JasonTwichell@coderfoundry.com",
                        Email = "JasonTwichell@coderfoundry.com",
                        FirstName = "Jason",
                        LastName = "Twichell"                       
                    };

                    userManager.Create(user, "Abc&123!");
                    userManager.AddToRoles(user.Id, "Admin");
                }                         

                if (!context.Users.Any(u => u.Email == "aRussell@coderfoundry.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "aRussell@coderfoundry.com",
                        Email = "aRussell@coderfoundry.com",
                        FirstName = "Drew",
                        LastName = "Russell"
                    };
                    userManager.Create(user, "Abc&123!");
                    userManager.AddToRoles(user.Id, "Admin");

                }

                #endregion
                #region Other Users & Roles

                if (!context.Users.Any(u => u.Email == "AFreeman@mailinator.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "AFreeman@mailinator.com",
                        Email = "AFreeman@mailinator.com",
                        FirstName = "Abagail",
                        LastName = "Freeman"
                    };
                    userManager.Create(user, "Test123!");
                    userManager.AddToRoles(user.Id, "ProjectManager");

                }

                if (!context.Users.Any(u => u.Email == "RFlagg@mailinator.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "RFlagg@mailinator.com",
                        Email = "RFlagg@mailinator.com",
                        FirstName = "Randall",
                        LastName = "Flagg"
                    };
                    userManager.Create(user, "Test123!");
                    userManager.AddToRoles(user.Id, "ProjectManager");

                }

                if (!context.Users.Any(u => u.Email == "FGoldsmith@Mailinator.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "FGoldsmith@Mailinator.com",
                        Email = "FGoldsmith@Mailinator.com",
                        FirstName = "Frannie",
                        LastName = "Goldsmith"
                    };
                    userManager.Create(user, "Test123!");
                    userManager.AddToRoles(user.Id, "Developer");

                }

                if (!context.Users.Any(u => u.Email == "NAndros@mailinator.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "NAndros@mailinator.com",
                        Email = "NAndros@mailinator.com",
                        FirstName = "Nick",
                        LastName = "Andros"
                    };
                    userManager.Create(user, "Test123!");
                    userManager.AddToRoles(user.Id, "Developer");

                }

                //if (!context.Users.Any(u => u.Email == "LloydHenreid@gmail.com"))
                //{
                //    var user = new ApplicationUser
                //    {
                //        UserName = "LloydHenreid@gmail.com",
                //        Email = "LloydHenreid@gmail.com",
                //        FirstName = "Lloyd",
                //        LastName = "Henreid"
                //    };
                //    userManager.Create(user, "Test123!");
                //    userManager.AddToRoles(user.Id, "Developer");
                //}

                if (!context.Users.Any(u => u.Email == "HLauder@mailinator.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "HLauder@mailinator.com",
                        Email = "HLauder@mailinator.com",
                        FirstName = "Harold",
                        LastName = "Lauder"
                    };
                    userManager.Create(user, "Test123!");
                    userManager.AddToRoles(user.Id, "Submitter");
                }

                if (!context.Users.Any(u => u.Email == "TCullen@mailinator.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "TCullen@mailinator.com",
                        Email = "TCullen@mailinator.com",
                        FirstName = "Tom",
                        LastName = "Cullen"
                    };
                    userManager.Create(user, "Test123!");
                    userManager.AddToRoles(user.Id, "Submitter");
                }


                //if (!context.Users.Any(u => u.Email == "SusanStern@gmail.com"))
                //{
                //    var user = new ApplicationUser
                //    {
                //        UserName = "SusanStern@gmail.com",
                //        Email = "SusanStern@gmail.com",
                //        FirstName = "Susan",
                //        LastName = "Stern"
                //    };
                //    userManager.Create(user, "Test123!");
                //    userManager.AddToRoles(user.Id, "Submitter");
                //}

                //if (!context.Users.Any(u => u.Email == "NadineCross@gmail.com"))
                //{
                //    var user = new ApplicationUser
                //    {
                //        UserName = "NadineCross@gmail.com",
                //        Email = "NadineCross@gmail.com",
                //        FirstName = "Nadine",
                //        LastName = "Cross"
                //    };
                //    userManager.Create(user, "Test123!");
                //    userManager.AddToRoles(user.Id, "Submitter");
            }
            #endregion

            #region Load Up Ticket Types
            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                     new TicketType { Name = "Defect" },
                     new TicketType { Name = "Display" },
                     new TicketType { Name = "Functionality" },
                    new TicketType { Name = "New Feature" }
                );

            #endregion
            #region Load Up Ticket Priorities
            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                     new TicketPriority { Name ="High" },
                     new TicketPriority { Name = "Medium" },
                     new TicketPriority { Name = "Low" }
                   
                );

            #endregion

            #region Load Up Ticket Status
            context.TicketStatus.AddOrUpdate(
                t => t.Name,
                     new TicketStatus { Name = "Unassigned" },
                     new TicketStatus { Name = "Assigned" },
                     new TicketStatus { Name = "Completed" },
                    new TicketStatus { Name = "Archived" }
                );

            #endregion
        }
    }
}
