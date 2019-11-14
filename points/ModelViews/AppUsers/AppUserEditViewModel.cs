using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace points.ModelViews.AppUsers
{
    public class AppUserEditViewModel
    {
          
        public AppUserEditViewModel()
        {

            Roles = new List<string>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "{0} مطلوب")]
        [EmailAddress]
        [Display(Name = "الإيميل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} مطلوب")]
        [Display(Name = "الاسم"), StringLength(100, ErrorMessage = " {0} " +
            "يجب ان يكون على الأقل " +
            "{2}" +
            " واعلى طول يكون " +
            "{1} حرف ", MinimumLength = 3)]
        public string FullName { get; set; }

        public IList<string> Roles { get; set; }
    }
}
