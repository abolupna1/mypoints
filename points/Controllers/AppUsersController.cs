using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using points.Data;
using points.Data.Repositories;
using points.Models;
using points.ModelViews.AppUsers;

namespace points.Controllers
{
    [Route("AppUsers")]
    [Authorize(Roles ="Admin")]
    public class AppUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AppUsersController> _logger;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IPointsRepository _repository;
        public AppUsersController(ApplicationDbContext context,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ILogger<AppUsersController> logger ,
             RoleManager<AppRole> roleManager, IPointsRepository repository)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
           _logger = logger;
            _roleManager = roleManager;
            _repository = repository;
        }

      
       

        public async Task<IActionResult> Index()
        {
            var model = new List<AppUserIndexModelViews>();
            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                var modelUser = new AppUserIndexModelViews()
                {
                    Id=user.Id,Email=user.Email,FullName=user.FullName
                };
                model.Add(modelUser);
            }

            return View(model);
        }

        [AcceptVerbs("Get", "Post")]
        [Route("IsEmailInUse")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"البريد الإلكتروني {email} مستخدم مسبقا");
            }
        }
    



        [Route("Create")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(AppUserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName=model.FullName,
                   
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
        
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction(nameof(Index));
                    }

                 
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {

            var appUser = await _context.Users.FindAsync(id);
            if (appUser == null)
            {
                ViewBag.ErrorMessage = "لايوجد مستخدم بهذه البيانات";
                return View("NotFound");
            }
            // GetRolesAsync returns the list of user Roles
            var userRoles = GetRolesAr(appUser.Id).Result;
            List<string> departmentsName = new List<string>();
            foreach (var depart in await _repository.GetAppUserDepartmentsByUserId(appUser.Id))
            {
                departmentsName.Add(depart.Department.Name);
            }

            var usertoedit = new AppUserEditViewModel()
            {
                FullName = appUser.FullName,
                Email = appUser.Email,
                Id = appUser.Id,
                Roles = userRoles,
                Departments = departmentsName
            };
            return View(usertoedit);
         
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id,AppUserEditViewModel model)
        {

            var appUser = await _context.Users.FindAsync(id);
            if (appUser == null)
            {
                ViewBag.ErrorMessage = "لايوجد مستخدم بهذه البيانات";
                return View("NotFound");
            }
            else
            {
               
                appUser.FullName = model.FullName;
             

                var result = await _userManager.UpdateAsync(appUser);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }
        }

        private async Task<List<string>> GetRolesAr(int userId)
        {
            List<string> arName = new List<string>();
            var user = await _context.Users.Include(u => u.UserRoles)

                .FirstOrDefaultAsync(u => u.Id == userId);
            foreach (var role in user.UserRoles)
            {
                var rolename = await _context.Roles.FindAsync(role.RoleId);
                if (rolename != null)
                {
                    arName.Add(rolename.ArName);

                }

            }
            return arName;
        }
        // POST: AppUsers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appUser = await _context.Users.FindAsync(id);
            if (appUser == null)
            {
                ViewBag.ErrorMessage = "لايوجد مستخدم بهذه البيانات";
                return View("NotFound");
            }
            _context.Users.Remove(appUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

   

        private bool AppUserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("ManageUserRoles/{userId:int}")]
        public async Task<IActionResult> ManageUserRoles(int userId)
        {
            ViewBag.userId = userId;

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"المستخدم  = {userId} غير موجود";
                return View("NotFound");
            }
            ViewBag.email = user.Email;
            var model = new List<AppUserRolesViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new AppUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    RoleArName = role.ArName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }
        [HttpPost]
        [Route("ManageUserRoles/{userId:int}")]
        public async Task<IActionResult> ManageUserRoles(List<AppUserRolesViewModel> model, int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                 ViewBag.ErrorMessage = "لايوجد مستخدم بهذه البيانات";
                return View("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "لم يتم حذف الأدوار");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "لم يتم اضافة الدوار المستخدمة");
                return View(model);
            }

            return RedirectToAction("Edit", new { id = userId });
        }



        [HttpGet]
        [Route("ManageUserDepartments/{userId:int}")]
        public async Task<IActionResult> ManageUserDepartments(int userId)
        {
            ViewBag.userId = userId;

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"المستخدم  = {userId} غير موجود";
                return View("NotFound");
            }
            var model = new List<AppUserDepartmentsViewModel>();
           
            foreach (var depart in await _repository.GetDepartments())
            {
                var userDepartmentsViewModel = new AppUserDepartmentsViewModel
                {
                  DepartmntId=depart.Id,
                  DepartmntName=depart.Name
                };

                if (await _repository.IsUserHasThisDepartment(user.Id, depart.Id))
                {
                    userDepartmentsViewModel.IsSelected = true;
                }
                else
                {
                    userDepartmentsViewModel.IsSelected = false;
                }

                model.Add(userDepartmentsViewModel);
            }

            return View(model);
        }


        [HttpPost]
        [Route("ManageUserDepartments/{userId:int}")]
        public async Task<IActionResult> ManageUserDepartments(int userId, List<AppUserDepartmentsViewModel> models)
        {
            ViewBag.userId = userId;

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"المستخدم  = {userId} غير موجود";
                return View("NotFound");
            }
            var depts = await _repository.GetAppUserDepartmentsByUserId(user.Id);
            if (depts.Count()>0)
            {
                foreach (var depart in depts)
                {
                    _repository.Delete<AppUserDepartment>(depart);

                }
                await _repository.SavaAll();
            }
      
            foreach (var dept in models)
            {
                if (dept.IsSelected)
                {
                    var model = new AppUserDepartment()
                    {
                        DepartmentId = dept.DepartmntId,
                        UserId = userId
                    };
                    _repository.Add<AppUserDepartment>(model);
                }
            
            }

            await _repository.SavaAll();

            return RedirectToAction("Edit", new { id = userId });
        }

    }
}
