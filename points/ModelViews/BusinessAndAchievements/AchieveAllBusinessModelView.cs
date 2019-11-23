using Microsoft.AspNetCore.Mvc;
using points.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace points.ModelViews.BusinessAndAchievements
{
    public class AchieveAllBusinessModelView
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "الموظف")]
        public int EmployeeId { get; set; }
  


        [Display(Name = "دورة قياس الأداء")]
        public int TimesOfEvaluationAndPerformanceId { get; set; }

        [Display(Name = "العمل")]
        public string TheWork { get; set; }
        [Display(Name = "وصف العمل")]
        public string TheWorkDescription { get; set; }

        [Display(Name = " التاريخ"),
    
     DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime WorkDate { get; set; }

        [Display(Name = "حالة انجاز العمل"), Required(ErrorMessage = "{0} مطلوب") ]
        public int? Status { get; set; }  // null not finished yet; 1 finish ; 3 Delayed  ; 2 Canceled

        [Display(Name = "ملاظات العمل الملغي او المؤجل")]
        public string NotesForWorkDelayed { get; set; }
       
        [Display(Name = "ملاظات ")]
        public string Notes { get; set; }
        
        [Display(Name = "سرعة اداء العمل ")]
        public int QuicklyPerformTheTask { get; set; }   // null not finished yet; 3 Fast ;2 medium ; 1 Weak;

    }
}
