using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{

    [MetadataType(typeof(TravelHistoryMetaData))]
    public partial class tbTravelHistory
    {
        
    }

    public class TravelHistoryMetaData
    {

        [Display(Name = "Viaje ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public int travel_ID { get; set; }

        [Display(Name = "Empleado ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public Nullable<int> employee_ID { get; set; }

        [Display(Name = "Subsidiaria ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public Nullable<int> subsidiary_ID { get; set; }

        [Display(Name = "Transportista ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public Nullable<int> transporter_ID { get; set; }

        [Display(Name = "Fecha de Salida")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]

        public Nullable<System.DateTime> departure_Date_and_Time { get; set; }


        [Display(Name = "Costo del Viaje")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public Nullable<decimal> travel_Cost { get; set; }


    }
}