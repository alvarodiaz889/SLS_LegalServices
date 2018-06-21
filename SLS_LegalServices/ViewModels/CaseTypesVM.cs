using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class CaseTypesVM
    {
        public int TypeId { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public string TypeCode { get; set; }

        public string FullDescription
        {
            get
            {
                var str = TypeCode + "-" + Description;
                str = (str.StartsWith("-") || str.EndsWith("-")) ? str.Replace("-", string.Empty) : str;
                return str;
            }
        }
    }
}