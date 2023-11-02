using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{

    [MetadataType(typeof(tbTravelMetaData))]

    public partial class tbTravel
    {

    }

    public class tbTravelMetaData
    {

        [Display(Name = "Viaje ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public int travel_ID { get; set; }

        [Display(Name = "Subsidiaria")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public Nullable<int> subsidiary_ID { get; set; }

        [Display(Name = "Transportista")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public Nullable<int> transporter_ID { get; set; }

        [Display(Name = "Empleado")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public Nullable<int> employee_ID { get; set; }

        [Display(Name = "Hora y Fecha de Salida")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public System.DateTime departure_Date_and_Time { get; set; }

        [Display(Name = "Distancia(KM)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public decimal distance_Kilometers { get; set; }

        [Display(Name = "Costo Total del Viaje")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public decimal total_travel_Cost { get; set; }


    }
}