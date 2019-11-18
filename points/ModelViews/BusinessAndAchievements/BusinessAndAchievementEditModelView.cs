using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace points.ModelViews.BusinessAndAchievements
{
    public class BusinessAndAchievementEditModelView
    {
        public int Id { get; set; }
        [Display(Name = "الموظف"), Required(ErrorMessage = "{0} مطلوب")]
        public int EmployeeId { get; set; }


        [Display(Name = "دورة قياس الأداء"), Required(ErrorMessage = "{0} مطلوب")]
        public int TimesOfEvaluationAndPerformanceId { get; set; }

        [Display(Name = "العمل"), StringLength(150), Required(ErrorMessage = "{0} مطلوب")]
        public string TheWork { get; set; }
        [Display(Name = "وصف العمل"), Required(ErrorMessage = "{0} مطلوب")]
        public string TheWorkDescription { get; set; }

        [Display(Name = " التاريخ"), Required(ErrorMessage = "{0} مطلوب"),
     DataType(DataType.Date, ErrorMessage = "المدخل يجب ان يكون تاريخ"),
     DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime WorkDate { get; set; }
    }
}
