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
    
    public partial class Log
    {
        public int LogId { get; set; }
        public string LogType { get; set; }
        public Nullable<int> CaseId { get; set; }
        public Nullable<System.DateTime> LogDate { get; set; }
        public Nullable<System.Guid> CreatedById { get; set; }
        public string Action { get; set; }
        public string Detail { get; set; }
        public Nullable<int> Active { get; set; }
    
        public virtual User User { get; set; }
        public virtual Case Case { get; set; }
    }
}
