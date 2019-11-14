using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;


namespace points.ModelViews.AppUsers
{
    public class AppUserCreateViewModel
    {
        [Required(ErrorMessage ="{0} مطلوب")]
        [EmailAddress]
        [Display(Name = "البريد الإلكتروني")]
        [Remote(action: "IsEmailInUse", controller: "AppUsers")]
     
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} مطلوب")]
        [Display(Name = "الاسم  ")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "{0} مطلوب")]
        [DataType(DataType.Password)]
        [Display(Name = " كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تاكيد كلمة المرور")]
        [Compare("Password",
            ErrorMessage = "{0}  غير متطابقة")]
        public string ConfirmPassword { get; set; }

     
    }
}

