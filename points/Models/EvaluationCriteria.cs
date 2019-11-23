using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace points.Models
{
    public class EvaluationCriteria
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 1)]
        public int Id { get; set; }

        [Display(Name = " المعيار "), Required(ErrorMessage = "{0} مطلوب")]
        public string CriteriaName { get; set; }

        [Display(Name = " النقاط "), Required(ErrorMessage = "{0} مطلوب")]
        public int Degree { get; set; }

        [Display(Name = "نوع الوظيفة "), Required(ErrorMessage = "{0} مطلوب")]
        public bool JopType { get; set; } // اشرافية 1

        public IEnumerable<Evaluation> Evaluations { get; set; }
    }
}
