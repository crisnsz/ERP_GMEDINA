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
    
    public partial class UDV_Inv_Proyeccion_de_Movimiento_de_Producto
    {
        public int bod_Id { get; set; }
        public string bod_Nombre { get; set; }
        public short bod_ResponsableBodega { get; set; }
        public string bod_Telefono { get; set; }
        public string prod_CodigoBarras { get; set; }
        public string prod_Descripcion { get; set; }
        public string prod_Marca { get; set; }
        public string prod_Modelo { get; set; }
        public string uni_Descripcion { get; set; }
        public string prod_Talla { get; set; }
        public decimal bodd_CantidadExistente { get; set; }
        public Nullable<decimal> Proyección_Días { get; set; }
    }
}
