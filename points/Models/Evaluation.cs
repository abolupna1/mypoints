using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace points.Models
{
    public class Evaluation
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


        [Display(Name = "معيار التقييم"), Required(ErrorMessage = "{0} مطلوب")]
        public int EvaluationCriteriaId { get; set; }
        [Display(Name = "معيار التقييم"), ForeignKey("EvaluationCriteriaId")]
        public EvaluationCriteria EvaluationCriteria { get; set; }


        [Display(Name = " النقاط "), Required(ErrorMessage = "{0} مطلوب")]
        public int Degree { get; set; }
    }
}
