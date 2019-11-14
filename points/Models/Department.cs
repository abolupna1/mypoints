using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace points.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} مطلوب")]
        [Display(Name = "الوكالة / الإدارة")]
        public string Name { get; set; }


        public IEnumerable<Section> Sections { get; set; }
        public IEnumerable<Unit> Units { get; set; }
        public IEnumerable<Employee> Employees { get; set; }


    }
}
