using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        [UIHint("DDLForGrid")]
        [AdditionalMetadata("DataValueField","Id")]
        [AdditionalMetadata("DataTextField", "Role")]
        [AdditionalMetadata("PropertyName", "Role")]
        [Display(Name = "Role")]
        [Required]
        public string Role { get; set; }

    }
}