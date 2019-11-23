using points.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace points.ModelViews.Reports
{
    public class DepartmentReportModelView
    {
        [Display(Name = "الإدارة")]
        public string Name { get; set; }

        public IEnumerable<BusinessAndAchievement> BusinessAndAchievements { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Occasion> Occasions { get; set; }
        public IEnumerable<Evaluation> Evaluations { get; set; }

    }
}
