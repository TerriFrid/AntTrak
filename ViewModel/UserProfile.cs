using AntTrak.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace AntTrak.ViewModel
{
    public class UserProfile
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string AvatarUrl{ get; set; }
        public string Role { get; set; }

        public string RoleId { get; set; }
        public int nbrOpenTickets { get; set; }
        
        public List<Project> MyProjects { get; set; }

        public string FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }
    }
}