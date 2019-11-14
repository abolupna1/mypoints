using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace points.Models
{
    public class Section
    {
        public int Id { get; set; }
        
        [Display(Name = "القسم"), Required(ErrorMessage = "{0} مطلوب")]
        public string Name { get; set; }
        [Display(Name = "الوكالة / الإدارة"), Required(ErrorMessage = "{0} مطلوب")]
        public int? DepartmentId { get; set; } 
        [Display(Name = "الوكالة / الإدارة"), ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public IEnumerable<Unit> Units { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}
