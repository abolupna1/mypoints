using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace points.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 1)]
        public int Id { get; set; }

        [Display(Name = "الموظف"), Required(ErrorMessage = "{0} مطلوب")]
        public int EmployeeId { get; set; }
        [Display(Name = "الموظف"), ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Display(Name = "دورة قياس الأداء"), Required(ErrorMessage = "{0} مطلوب")]
        public int TimesOfEvaluationAndPerformanceId { get; set; }
        [Display(Name = "دورة قياس الأداء"), ForeignKey("TimesOfEvaluationAndPerformanceId")]
        public TimesOfEvaluationAndPerformance TimesOfEvaluationAndPerformance { get; set; }

        [Display(Name = "عنوان الدورة"), Required(ErrorMessage = "{0} مطلوب")]
        public string Title { get; set; }
        [Display(Name = "النقاط")]
        public int Degree { get; set; } = 3;

        [Display(Name = " التاريخ"), Required(ErrorMessage = "{0} مطلوب"),
        DataType(DataType.Date, ErrorMessage = "المدخل يجب ان يكون تاريخ"),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CourseDate { get; set; }
        [Display(Name = "ملاحظات")]
        public string Notce { get; set; } = null;

    }
}
