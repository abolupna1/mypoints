using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using points.Models;
namespace points.ModelViews.BusinessAndAchievements
{
    public class EmployeeBusinessModelView
    {
       
            public int Id { get; set; }

            [Display(Name = "الإسم")]
            public string Name { get; set; }

            [Display(Name = "رقم الوظف")]
            public int EmployeeNo { get; set; }

            [Display(Name = "رقم الجوال")]
            public string Mobile { get; set; }

            [Display(Name = "نوع الوظيفة ")]
            public string JopType { get; set; } // اشرافية 1
            [Display(Name = "الوظيفة ")]
            public string JopName { get; set; }




            [Display(Name = "الوكالة / الإدارة")]
            public string Department { get; set; }


            [Display(Name = " القسم")]
            public string Section { get; set; }


            [Display(Name = "الوحدة")]
            public string Unit { get; set; } = null;

        [Display(Name = "الوحدة")]
        public int MyPoints { get; set; } = 0;

        public IEnumerable<BusinessAndAchievement> BusinessAndAchievements { get; set; }
            public IEnumerable<Course> Courses { get; set; }
            public IEnumerable<Occasion> Occasions { get; set; }
            public IEnumerable<Evaluation> Evaluations { get; set; }


    }
}
