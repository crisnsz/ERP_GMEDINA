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
    
    public partial class UDP_Vent_tbMovimientoCaja_Imprimir_Result
    {
        public int mocja_Id { get; set; }
        public Nullable<System.DateTime> mocja_FechaApertura { get; set; }
        public int mocja_UsuarioApertura { get; set; }
        public Nullable<System.DateTime> mocja_FechaArqueo { get; set; }
        public int mocja_UsuarioArquea { get; set; }
        public Nullable<System.DateTime> mocja_FechaAceptacion { get; set; }
        public int mocja_UsuarioAceptacion { get; set; }
        public string cja_Descripcion { get; set; }
        public string suc_Descripcion { get; set; }
        public Nullable<short> arqde_CantidadDenominacion { get; set; }
        public Nullable<decimal> arqde_MontoDenominacion { get; set; }
        public string deno_Descripcion { get; set; }
        public Nullable<decimal> deno_valor { get; set; }
        public Nullable<decimal> arqpg_PagosSistema { get; set; }
        public Nullable<decimal> arqpg_PagosConteo { get; set; }
        public string tpa_Descripcion { get; set; }
        public string Usuario_Apertura { get; set; }
        public string Usuario_Arquea { get; set; }
        public string Usuario_Aceptacion { get; set; }
    }
}
