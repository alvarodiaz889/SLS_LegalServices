using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class GenericValuesLookupVM
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        private string _display;
        public string Display {
            get { return _display ?? Value; }
            set { _display = value; }
        }
    }
}