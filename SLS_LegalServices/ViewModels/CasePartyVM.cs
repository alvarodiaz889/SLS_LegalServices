﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLS_LegalServices.ViewModels
{
    public class CasePartyVM
    {
        public int CasePartyId { get; set; }

        public int CaseId { get; set; }

        [UIHint("PartyTypesDDL")]
        [Display(Name = "Type")]
        public string PartyType { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string OrganizationName { get; set; }

        public int AddressId { get; set; }

        public int EmailId { get; set; }

        [UIHint("YesNoDropDown")]
        [AdditionalMetadata("Name", "IsIUStudent")]
        [Display(Name = "IU Student")]
        public bool IsIUStudent { get; set; }

        [UIHint("EditEmail")]
        [Display(Name =" ")]
        public EmailVM Email { get; set; }

        [UIHint("EditAddress")]
        [Display(Name = " ")]
        public AddressVM Address { get; set; }
    }
}