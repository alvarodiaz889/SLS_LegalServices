using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class AttorneyVM
    {
        public int AttorneyId { get; set; }
        public Guid UserId { get; set; }

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

        public UserViewModel UserVM { get; set; }
        
    }
}