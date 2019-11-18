using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace points.Models
{
    public class BusinessAndAchievement
    {
      
        public int Id { get; set; }


        [Display(Name = "الموظف"), Required(ErrorMessage = "{0} مطلوب")]
        public int EmployeeId { get; set; }
        [Display(Name = "الموظف"), ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Display(Name = "دورة قياس الأداء"), Required(ErrorMessage = "{0} مطلوب")]
        public int TimesOfEvaluationAndPerformanceId { get; set; }
        [Display(Name = "دورة قياس الأداء"), ForeignKey("TimesOfEvaluationAndPerformanceId")]
        public TimesOfEvaluationAndPerformance TimesOfEvaluationAndPerformance { get; set; }

        [Display(Name = "العمل"),StringLength(150), Required(ErrorMessage = "{0} مطلوب")]
        public string TheWork { get; set; }
        [Display(Name = "وصف العمل"),  Required(ErrorMessage = "{0} مطلوب")]
        public string TheWorkDescription { get; set; }

        [Display(Name = " التاريخ"), Required(ErrorMessage = "{0} مطلوب"),
     DataType(DataType.Date, ErrorMessage = "المدخل يجب ان يكون تاريخ"),
     DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime WorkDate { get; set; }

        [Display(Name = "حالة انجاز العمل")]
        public int? Status { get; set; } = null; // null not finished yet; 1 finish ; 0 Delayed or Canceled
        [Display(Name = "ملاظات العمل الملغي او المؤجل")]
        public string NotesForWorkDelayed { get; set; } = null;
        [Display(Name = "ملاظات ")]
        public string Notes { get; set; } = null;

        [Display(Name = "سرعة اداء العمل ")]
        public int? QuicklyPerformTheTask { get; set; } = null;  // null not finished yet; 3 Fast ;2 medium ; 1 Weak;

    }
}
