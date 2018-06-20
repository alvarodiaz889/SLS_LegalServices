using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class UserViewModel
    {
        public System.Guid UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        public string DisplayName { get; set; }

        public bool? Active { get; set; }

        [UIHint("Roles")]
        [Display(Name = "Roles")]
        [Required]
        public string Role { get; set; }

    }
}