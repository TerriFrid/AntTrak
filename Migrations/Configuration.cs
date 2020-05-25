namespace AntTrak.Migrations
{
    using AntTrak.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

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
            }
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
            #region  TEST USERS
            //Admin
            AddUser(new ApplicationUser
            {
                UserName = "admin@mailinator.com",
                Email = "admin@mailinator.com",
                FirstName =  "Test",
                LastName = "Admin",
                AvatarPath = "Avatars/blank-avatar.jpg",
                EmailConfirmed = true
            }, context, "Admin");

            //ProjectManager
            AddUser(new ApplicationUser
            {
                UserName = "projectmanager@mailinator.com",
                Email = "projectmanager@mailinator.com",
                FirstName = "Test",
                LastName = "ProjectManager",
                AvatarPath = "Avatars/blank-avatar.jpg",
                EmailConfirmed = true
            }, context, "ProjectManager");

            //Developer
            AddUser(new ApplicationUser
            {
                UserName = "developer@mailinator.com",
                Email = "developer@mailinator.com",
                FirstName = "Test",
                LastName = "Developer",
                AvatarPath = "Avatars/blank-avatar.jpg",
                EmailConfirmed = true
            }, context, "Developer");

            //Submitter
            AddUser(new ApplicationUser
            {
                UserName = "submitter@mailinator.com",
                Email = "submitter@mailinator.com",
                FirstName = "Test",
                LastName = "Submitter",
                AvatarPath = "Avatars/blank-avatar.jpg",
                EmailConfirmed = true
            }, context, "Submitter");
            
            #endregion

            #region Add Custom Users & Roles
            
            //var userStore = new UserStore<Models.ApplicationUser>(context);
            //var userManager = new UserManager<Models.ApplicationUser>(userStore);


            //if (!context.Users.Any(u => u.Email == "terriafsusa@gmail.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "terriafsusa@gmail.com",
            //        Email = "terriafsusa@gmail.com",
            //        FirstName = "Terri",
            //        LastName = "Frid"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "Admin");
            //}

            //if (!context.Users.Any(u => u.Email == "JasonTwichell@coderfoundry.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "JasonTwichell@coderfoundry.com",
            //        Email = "JasonTwichell@coderfoundry.com",
            //        FirstName = "Jason",
            //        LastName = "Twichell"
            //    };

            //    userManager.Create(user, "Abc&123!");
            //    userManager.AddToRoles(user.Id, "Admin");
            //}

            //if (!context.Users.Any(u => u.Email == "aRussell@coderfoundry.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "aRussell@coderfoundry.com",
            //        Email = "aRussell@coderfoundry.com",
            //        FirstName = "Drew",
            //        LastName = "Russell"
            //    };
            //    userManager.Create(user, "Abc&123!");
            //    userManager.AddToRoles(user.Id, "Admin");

            //}

            //if (!context.Users.Any(u => u.Email == "AFreeman@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "AFreeman@mailinator.com",
            //        Email = "AFreeman@mailinator.com",
            //        FirstName = "Abagail",
            //        LastName = "Freeman"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "ProjectManager");

            //}

            //if (!context.Users.Any(u => u.Email == "RFlagg@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "RFlagg@mailinator.com",
            //        Email = "RFlagg@mailinator.com",
            //        FirstName = "Randall",
            //        LastName = "Flagg"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "ProjectManager");

            //}

            //if (!context.Users.Any(u => u.Email == "SRedman@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "SRedman@mailinator.com",
            //        Email = "SRedman@mailinator.com",
            //        FirstName = "Stuart",
            //        LastName = "Redman"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "ProjectManager");
            //}

            //if (!context.Users.Any(u => u.Email == "GBateman@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "GBateman@mailinator.com",
            //        Email = "GBateman@mailinator.com",
            //        FirstName = "Glen",
            //        LastName = "Bateman"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "ProjectManager");
            //}

            //if (!context.Users.Any(u => u.Email == "WHorgan@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "WHorgan@mailinator.com",
            //        Email = "WHorgan@mailinator.com",
            //        FirstName = "Whitney",
            //        LastName = "Horgan"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "ProjectManager");

            //}

            //if (!context.Users.Any(u => u.Email == "FGoldsmith@Mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "FGoldsmith@Mailinator.com",
            //        Email = "FGoldsmith@Mailinator.com",
            //        FirstName = "Frannie",
            //        LastName = "Goldsmith"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "Developer");

            //}

            //if (!context.Users.Any(u => u.Email == "NAndros@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "NAndros@mailinator.com",
            //        Email = "NAndros@mailinator.com",
            //        FirstName = "Nick",
            //        LastName = "Andros"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "Developer");

            //}

            //if (!context.Users.Any(u => u.Email == "LHenreid@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "LHenreid@mailinator.com",
            //        Email = "LHenreid@mailinator.com",
            //        FirstName = "Lloyd",
            //        LastName = "Henreid"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "Developer");
            //}

            //if (!context.Users.Any(u => u.Email == "DJurgens@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "DJurgens@mailinator.com",
            //        Email = "DJurgens@mailinator.com",
            //        FirstName = "Dayna",
            //        LastName = "Jurgens"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "Developer");
            //}

            //if (!context.Users.Any(u => u.Email == "NCross@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "NCross@mailinator.com",
            //        Email = "NCross@mailinator.com",
            //        FirstName = "Nadine",
            //        LastName = "Cross"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "Developer");
            //}


            //if (!context.Users.Any(u => u.Email == "CCampion@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "CCampion@mailinator.com",
            //        Email = "CCampion@mailinator.com",
            //        FirstName = "Charlie",
            //        LastName = "Campion"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "Developer");
            //}


            //if (!context.Users.Any(u => u.Email == "HLauder@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "HLauder@mailinator.com",
            //        Email = "HLauder@mailinator.com",
            //        FirstName = "Harold",
            //        LastName = "Lauder"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "Submitter");
            //}

            //if (!context.Users.Any(u => u.Email == "TCullen@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "TCullen@mailinator.com",
            //        Email = "TCullen@mailinator.com",
            //        FirstName = "Tom",
            //        LastName = "Cullen"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "Submitter");
            //}


            //if (!context.Users.Any(u => u.Email == "SusanStern@mailinator.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "SusanStern@mailinator.com",
            //        Email = "SusanStern@mailinator.com",
            //        FirstName = "Susan",
            //        LastName = "Stern"
            //    };
            //    userManager.Create(user, "Test123!");
            //    userManager.AddToRoles(user.Id, "Submitter");
            //}

            #endregion

            #region RANDOM USERS
            //create 5 Project Managers
            for (int pm = 0; pm < 5; pm++)
            {
                AddUser(CreateRandomUser(), context, "ProjectManager");
            }
            //create 10 Developers
            for (int pm = 0; pm< 10; pm++)
            {
                AddUser(CreateRandomUser(), context, "Developer");
            }

            //create 5 Submitterss
            for (int pm = 0; pm < 5; pm++)
            {
                AddUser(CreateRandomUser(), context, "Submitter");
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
                         new TicketPriority { Name = "High" },
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

            context.SaveChanges();

            CreateProjects(20);
            CreateTickets(10);

           
        }
        public void AddUser(ApplicationUser user, ApplicationDbContext db, string role)
                {
                    var userStore = new UserStore<ApplicationUser>(db);
                    var userManager = new UserManager<ApplicationUser>(userStore);

                    if(!db.Users.Any(u=> u.Email == user.Email))
                    {
                        userManager.Create(user, "Test123!");
                        userManager.AddToRoles(user.Id, role);
                    }
                }
        public ApplicationUser CreateRandomUser()
        {
            List<string> firstNames = new List<string> { "Michael", "Ethan", "Peter", "Josh", "Jace",
            "Ben", "Katie", "Craig", "Trevor", "Allison", "Lawson", "Brian", "TK", "Shane", "Chris",
            "Ashton", "Terri", "Jaylon", "Matt"};
            List<string> lastNames = new List<string> { "Carreno", "Nance", "Fralick", "Casteel", "Matthews",
            "Yarema", "Rosario", "Hoffman", "Popham", "Popham", "Tsatsa", "Ott", "Quinn", "McMiller",
            "Corthum", "Fulp", "Wray", "Frid", "Martin", "Wendel"};

            Random rand = new Random();

            var first = firstNames[rand.Next(0, firstNames.Count)];
            var last = lastNames[rand.Next(0, lastNames.Count)];
            var email = $"{first}.{last}@mailinator.com";

            return new ApplicationUser
            {
                UserName = email, 
                Email = email,
                FirstName = first,
                LastName = last, 
                AvatarPath = "Avatars/blank-avatar.jpg",
                EmailConfirmed = true
            };
        }

        public void CreateProjects(int numberOfProjects)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var rand = new Random();
            var rolesHelper = new AntTrak.Helpers.UserRolesHelper();
            var projHelper = new AntTrak.Helpers.ProjectHelper();
            var projManagers = rolesHelper.UsersInRole("ProjectManager").ToList();

            for (int i = 1; i <= 20; i++)
            {
                var seededProjectName = $"Project {21 - i} (Seeded)";
                db.Projects.AddOrUpdate(p => p.Name, new Project
                {
                    Name = seededProjectName,
                    Description = "This is a demo Project that has been seeded.",
                    Created = DateTime.Now.AddDays(-i *3),
                    ProjectManagerId = projManagers[rand.Next(0,projManagers.Count)].Id
                });
            }
            db.SaveChanges();
        } 
        
        public void CreateTickets (int numOfTickets)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Random rand = new Random();
            var rolesHelper = new AntTrak.Helpers.UserRolesHelper();
            var projHelper = new AntTrak.Helpers.ProjectHelper();

            var developers = rolesHelper.UsersInRole("Developer").ToList();
            var submitters = rolesHelper.UsersInRole("Submitter").ToList();

            var seedTicketType = db.TicketTypes.Select(t => t.Id).ToList();
            var seedTicketPriority = db.TicketPriorities.Select(t => t.Id).ToList();
            var seedTicketStatus = db.TicketStatus.FirstOrDefault(t => t.Name == "Unassigned").Id;

            var projects = db.Projects.ToList();

            foreach (var project in projects)
            {
                var seedSub = submitters[rand.Next(0, submitters.Count)];
                projHelper.AddUserToProject(seedSub.Id, project.Id);

               for (int j = 1; j <= 10; j++)
                {
                    db.Tickets.AddOrUpdate(t => t.Title, new Ticket
                    {
                        Title = $"Ticket {db.Tickets.Count() + 1}",
                        ProjectId = project.Id,
                        Description = "This is a seeded demo Ticket.",
                        TicketTypeId = seedTicketType[rand.Next(0, seedTicketType.Count)],
                        TicketPriorityId = seedTicketPriority[rand.Next(0, seedTicketPriority.Count)],
                        TicketStatusId = seedTicketStatus,
                        SubmitterId =  seedSub.Id,
                        Created = DateTime.Now
                    });
                }
                db.SaveChanges();

            }
        }
        
    }
}
