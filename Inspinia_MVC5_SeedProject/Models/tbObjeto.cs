//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP_GMEDINA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbObjeto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbObjeto()
        {
            this.tbAccesoRol = new HashSet<tbAccesoRol>();
        }
    
        public int obj_Id { get; set; }
        public string obj_Pantalla { get; set; }
        public string obj_Referencia { get; set; }
        public int obj_UsuarioCrea { get; set; }
        public System.DateTime obj_FechaCrea { get; set; }
        public Nullable<int> obj_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> obj_FechaModifica { get; set; }
        public bool obj_Estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbAccesoRol> tbAccesoRol { get; set; }
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
    }
}
