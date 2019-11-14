using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace points.ModelViews.AppUsers
{
    public class AppUserRolesViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleArName { get; set; }
        public bool IsSelected { get; set; }
    }
}
