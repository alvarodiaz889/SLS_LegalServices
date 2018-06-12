using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class AddressVM
    {
        public int AddressId { get; set; }
        public string Type { get; set; }
        public int? CaseId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public CaseVM Case { get; set; }
        public string FullAddress { get; set; }

    }
}