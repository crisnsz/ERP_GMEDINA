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
    
    public partial class tbEmployee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbEmployee()
        {
            this.tbTravelHistories = new HashSet<tbTravelHistory>();
            this.tbUsers = new HashSet<tbUser>();
            this.tbEmployeesSubsidiaries = new HashSet<tbEmployeesSubsidiary>();
        }
    
        public int employee_ID { get; set; }
        public string employee_Name { get; set; }
        public string employee_Direction { get; set; }
        public Nullable<int> position_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbTravelHistory> tbTravelHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbUser> tbUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbEmployeesSubsidiary> tbEmployeesSubsidiaries { get; set; }
        public virtual tbPosition tbPosition { get; set; }
    }
}
