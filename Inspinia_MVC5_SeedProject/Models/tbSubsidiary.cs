//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP_GMEDINA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbSubsidiary
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbSubsidiary()
        {
            this.tbEmployeesSubsidiaries = new HashSet<tbEmployeesSubsidiary>();
            this.tbTravelHistories = new HashSet<tbTravelHistory>();
        }
    
        public int subsidiary_ID { get; set; }
        public string subsidiary_Name { get; set; }
        public string subsidiary_Direction { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbEmployeesSubsidiary> tbEmployeesSubsidiaries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbTravelHistory> tbTravelHistories { get; set; }
    }
}
