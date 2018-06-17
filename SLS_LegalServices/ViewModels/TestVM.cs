using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLS_LegalServices.ViewModels
{
    public class TestVM
    {
        public int TestId { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}