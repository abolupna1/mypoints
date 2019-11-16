using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace points.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<AppUserDepartment> UserDepatrments { get; set; }
    }
}
