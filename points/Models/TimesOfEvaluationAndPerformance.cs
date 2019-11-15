using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace points.Models
{
    public class TimesOfEvaluationAndPerformance
    {
        public int Id { get; set; }
        [Display(Name = "العنوان"), Required(ErrorMessage = "{0} مطلوب")]
        public string Title { get; set; }
        [Display(Name = "من تاريخ"), Required(ErrorMessage = "{0} مطلوب"),
         DataType(DataType.Date, ErrorMessage = "المدخل يجب ان يكون تاريخ"),
         DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }

        [Display(Name = "الى تاريخ"), Required(ErrorMessage = "{0} مطلوب"),
       DataType(DataType.Date, ErrorMessage = "المدخل يجب ان يكون تاريخ"),
       DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }

        [Display(Name = "الحالة"), Required(ErrorMessage = "{0} مطلوب")]
        public bool Status { get; set; } 
    }
}
