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
    
    public partial class UDP_Vent_tbSolicitudEfectivo_Imprimir_Result
    {
        public int solef_Id { get; set; }
        public string cja_Descripcion { get; set; }
        public string suc_Descripcion { get; set; }
        public int mocja_Id { get; set; }
        public string TipoSolicitus { get; set; }
        public Nullable<System.DateTime> solef_FechaEntrega { get; set; }
        public string UsuarioEntrega { get; set; }
        public string Cajero { get; set; }
        public string mnda_Nombre { get; set; }
        public int soled_Id { get; set; }
        public short deno_Id { get; set; }
        public string deno_Descripcion { get; set; }
        public decimal deno_valor { get; set; }
        public short soled_CantidadSolicitada { get; set; }
        public Nullable<decimal> MontoSolicitado { get; set; }
        public short soled_CantidadEntregada { get; set; }
        public Nullable<decimal> MontoEntregado { get; set; }
    }
}
