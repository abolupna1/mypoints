using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace points.ModelViews.AppUsers
{
    public class AppUserDepartmentsViewModel
    {
        public int DepartmntId { get; set; }
        [Display(Name = "الإدارة")]

        public string DepartmntName { get; set; }
        public bool IsSelected { get; set; }
    }
}
