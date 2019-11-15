using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace points.ModelViews.Employees
{
    public class EmployeeEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "الإسم"), Required(ErrorMessage = "{0} مطلوب")]
        public string Name { get; set; }

        [Display(Name = "رقم الوظف"), Required(ErrorMessage = "{0} مطلوب")]
        [Remote(action: "IsEmployeeNomberInUseForEdit", controller: "Employees",AdditionalFields ="Id")]
        public int EmployeeNo { get; set; }

        [Display(Name = "رقم الجوال"), Required(ErrorMessage = "{0} مطلوب")]
        public string Mobile { get; set; }

        [Display(Name = "نوع الوظيفة "), Required(ErrorMessage = "{0} مطلوب")]
        public bool? JopType { get; set; } // اشرافية 1
        [Display(Name = "الوظيفة "), Required(ErrorMessage = "{0} مطلوب")]
        public string JopName { get; set; }

        [Display(Name = "الوكالة / الإدارة"), Required(ErrorMessage = "{0} مطلوب")]
        public int? DepartmentId { get; set; }


        [Display(Name = " القسم")]
        public int? SectionId { get; set; } = null;


        [Display(Name = "الوحدة")]
        public int? UnitId { get; set; } = null;

    }
}
