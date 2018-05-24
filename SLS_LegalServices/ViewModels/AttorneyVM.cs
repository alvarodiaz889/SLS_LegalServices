using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class AttorneyModel
    {
        public System.Guid UserId { get; set; }

        [Required]
        [MaxLength(50)]
		[Display(Name = "User ID")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        public string DisplayName { get; set; }

        public bool? Active { get; set; }
        
    }
}