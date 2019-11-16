using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace points.Models
{
    public class AppUserDepartment
    {

     
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 1)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

    }
}
