using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [UIHint("Roles")]
        [Display(Name = "Roles")]
        [Required]
        public List<RoleViewModel> Roles { get; set; }

        [Display(Name = "Roles")]
        public string UserRoles { get; set; }
    }
}