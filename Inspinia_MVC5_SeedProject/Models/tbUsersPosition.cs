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
    
    public partial class tbUsersPosition
    {
        public int userPosition_ID { get; set; }
        public Nullable<int> position_ID { get; set; }
        public Nullable<int> user_ID { get; set; }
    
        public virtual tbPosition tbPosition { get; set; }
        public virtual tbUser tbUser { get; set; }
    }
}
