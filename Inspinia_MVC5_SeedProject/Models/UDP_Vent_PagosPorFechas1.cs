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
    
    public partial class UDP_Vent_PagosPorFechas1
    {
        public int Num__Linea { get; set; }
        public string RTN_Cliente { get; set; }
        public string Nombre_Completo { get; set; }
        public string Código_Factura { get; set; }
        public string Tipo_Pago { get; set; }
        public decimal Saldo_Anterior { get; set; }
        public decimal Total_Pago { get; set; }
        public Nullable<decimal> Saldo_Actual { get; set; }
    }
}
