//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SLS_LegalServices
{
    using System;
    using System.Collections.Generic;
    
    public partial class CaseIntern
    {
        public int InternId { get; set; }
        public int CaseId { get; set; }
        public System.DateTime CreationDate { get; set; }
    
        public virtual Intern Intern { get; set; }
        public virtual Case Case { get; set; }
    }
}