
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using points.Data.Repositories;
using points.Models;
using points.ModelViews.Employees;

namespace points.Controllers
{
    [Route("Employees")]
    [Authorize(Roles = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly IPointsRepository _repository;
        private readonly IMapper _mapper;

        public EmployeesController(IPointsRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [AcceptVerbs("Get", "Post")]
        [Route("IsEmployeeNomberInUse")]
        public async Task<IActionResult> IsEmployeeNomberInUse(int EmployeeNo)
        {
          

            if (await _repository.IsEmployeeNomberInUse(EmployeeNo))
            {
                return Json($"رقم الموظف {EmployeeNo} موجود مسبقا");

            }
            else
            {
                return Json(true);

            }
          
        }
        [AcceptVerbs("Get", "Post")]
        [Route("IsEmployeeNomberInUseForEdit")]
        public async Task<IActionResult> IsEmployeeNomberInUseForEdit(int EmployeeNo ,int Id)
        {


            if (await _repository.IsEmployeeNomberInUseForEdit(EmployeeNo,Id))
            {
                return Json($"رقم الموظف {EmployeeNo} موجود مسبقا");

            }
            else
            {
                return Json(true);

            }

        }

        

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetEmployees());
        }

        [Route("Create")]
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name");
            ViewData["SectionId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name");
            ViewData["UnitId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(EmployeeCreateViewModel employee)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.IsEmployeeNomberInUse(employee.EmployeeNo))
                {
                    ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", employee.DepartmentId);
                    ViewBag.errormessage = $"رقم الوظف {employee.EmployeeNo} موجود مسبقا";
                    return View(employee);
                }
                var employeeMapper = _mapper.Map<Employee>(employee);
                _repository.Add<Employee>(employeeMapper);
                await _repository.SavaAll();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", employee.DepartmentId);
            ViewData["SectionId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name",employee.SectionId);
            ViewData["UnitId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name",employee.UnitId);
            return View(employee);
        }

        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {


            var employee = await _repository.GetEmployee(id);
            if (employee == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", employee.DepartmentId);
            ViewData["SectionId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", employee.SectionId);
            ViewData["UnitId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", employee.UnitId);
            var employeeMapper = _mapper.Map<EmployeeEditViewModel>(employee);

            return View(employeeMapper);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, EmployeeEditViewModel employee)
        {
            if (id != employee.Id)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var employeeMapper = _mapper.Map<Employee>(employee);

                    _repository.Update<Employee>(employeeMapper);
                    await _repository.SavaAll();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_repository.GetEmployee(employee.Id) == null)
                    {
                        ViewBag.ErrorMessage = "لايوجد   بيانات";
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", employee.DepartmentId);
            ViewData["SectionId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", employee.SectionId);
            ViewData["UnitId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", employee.UnitId);
            return View(employee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _repository.GetEmployee(id);
            if (employee == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            try
            {
                _repository.Delete<Employee>(employee);
                await _repository.SavaAll();


            }
            catch (DbUpdateConcurrencyException)
            {
                ViewBag.ErrorMessage = $"توجد حقول مرتبطة بهذا الموظف /{employee.Name} الرجاء حذف جميع الحقول المرتبطة من ثم  يمكن حذف الموظف";
                return View("NotFound");


            }


            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("GetSectionsByDepartmentId/{departmentId:int}")]
        public async Task<IEnumerable<Section>> GetSectionsByDepartmentId(int departmentId)
        {
            return await _repository.GetSectionsByDepartmentId(departmentId);
        }
        [HttpGet]
        [Route("GetUnitsBySectionIdAndDepartmentId/{departmentId:int}/{sectionId:int}")]
        public async Task<IEnumerable<Unit>> GetUnitsBySectionIdAndDepartmentId(int departmentId, int sectionId)
        {   
            return await _repository.GetUnitsBySectionIdAndDepartmentId(departmentId, sectionId);
        }
        
    }
}
