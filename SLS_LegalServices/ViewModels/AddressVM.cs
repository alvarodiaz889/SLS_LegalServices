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
        public string FullAddress {
            get {
                string fullAddress = string.Empty;
                fullAddress += Address1 ?? string.Empty;
                fullAddress += (string.IsNullOrEmpty(Address2)) ? string.Empty : ", " + Address2;
                fullAddress += (string.IsNullOrEmpty(City)) ? string.Empty : ", " + City;
                fullAddress += (string.IsNullOrEmpty(State)) ? string.Empty : ", " + State;
                fullAddress += (string.IsNullOrEmpty(PostalCode)) ? string.Empty : ", " + PostalCode;
                fullAddress += (string.IsNullOrEmpty(Country)) ? string.Empty : ", " + Country;
                return fullAddress;
            }
        }

    }
}