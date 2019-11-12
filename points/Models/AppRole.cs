using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace points.Models
{
    public class AppRole:IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
        public string ArName { get; set; }
        public AppRole() : base() { }
        public AppRole(string roleName) : base(roleName) { }
        public AppRole(string roleName, string arName) : base(roleName)
        {
            this.ArName = arName;

        }
    }
}
