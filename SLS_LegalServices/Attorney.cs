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
    
    public partial class Attorney
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Attorney()
        {
            this.Intern_Attorney = new HashSet<Intern_Attorney>();
        }
    
        public int AttorneyId { get; set; }
        public System.Guid UserId { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Intern_Attorney> Intern_Attorney { get; set; }
    }
}
