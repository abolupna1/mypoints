using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using points.Data;
using points.Data.Repositories;
using points.Models;
using points.ModelViews.BusinessAndAchievements;

namespace points.Controllers
{
    [Route("BusinessAndAchievements")]

    public class BusinessAndAchievementsController : Controller
    {
            private readonly IPointsRepository _repository;
        private readonly IMapper _mapper;

        public BusinessAndAchievementsController(IPointsRepository repository, IMapper mapper)
            {
                _repository = repository;
            _mapper = mapper;
        }

            public async Task<IActionResult> Index()
            {
                return View(await _repository.GetTimesOfEvaluationAndPerformances());
            }


    

        [Route("Employees/{departmentId:int}/{timesOfEvaluationAndPerformanceId:int}")]
            public async Task<IActionResult> Employees(int departmentId,int timesOfEvaluationAndPerformanceId)
            {
                var timesOf = await _repository.GetTimesOfEvaluationAndPerformance(timesOfEvaluationAndPerformanceId);
                if (timesOf == null)
                {
                    ViewBag.ErrorMessage = "لايوجد   بيانات";
                    return View("NotFound");
                }
                 var department = await _repository.GetDepartment(departmentId);
                  ViewBag.department = department != null ? department.Name: string.Empty;
                  ViewBag.timesOfEvaluationAndPerformanceId = timesOfEvaluationAndPerformanceId;
                 ViewBag.departmentId = departmentId;
            var employees = await _repository.GetEmployeesByDpartmentId(departmentId);
                return View(employees);
            }

        [Route("WorksByEmployee/{employeeId:int}/{timesOfEvaluationAndPerformanceId:int}")]
        public async Task<IActionResult> WorksByEmployee(int employeeId, int timesOfEvaluationAndPerformanceId)
        {
            ViewBag.employeeId = employeeId;
            var emploee = await _repository.GetEmployee(employeeId);
            ViewBag.departmentId = emploee != null ? emploee.DepartmentId : 0;
            ViewBag.employeeName = emploee != null ? emploee.Name : "";
            ViewBag.timesOfEvaluationAndPerformanceId = timesOfEvaluationAndPerformanceId;
            return View(await _repository.GetWorksByEmployeeIdAndTimesOf(employeeId, timesOfEvaluationAndPerformanceId));
        }

     
        [Route("CreateGet")]
        public async Task<IActionResult> CreateGet(int employeeId, int countCreate, int departmentId, int timesOfEvaluationAndPerformanceId)
            {
          
                ViewBag.employeeId = employeeId;
                var emploee = await _repository.GetEmployee(employeeId);
                ViewBag.employeeName = emploee != null ? emploee.Name : "";
                ViewBag.departmentId = departmentId;
                ViewBag.countCreate = countCreate;
                ViewBag.timesOfEvaluationAndPerformanceId = timesOfEvaluationAndPerformanceId;
                return View();
            }




            [HttpPost]
            [ValidateAntiForgeryToken]
            [Route("Create")]
            public async Task<IActionResult> Create(List<BusinessAndAchievementCreateModelView> models)
            {
                if (ModelState.IsValid)
                {
                var modelsMapper = _mapper.Map<List<BusinessAndAchievement>>(models);
                int empId = 0;
                int timeOf = 0;
                foreach (var model in modelsMapper)
                {
                    _repository.Add<BusinessAndAchievement>(model);
                    empId = model.EmployeeId;
                    timeOf = model.TimesOfEvaluationAndPerformanceId;
                }
                await _repository.SavaAll();
                    return RedirectToAction("WorksByEmployee", new { employeeId=empId, timesOfEvaluationAndPerformanceId= timeOf });
                }
                return View(models);
            }

            [Route("Edit/{id:int}")]
            public async Task<IActionResult> Edit(int id)
            {


                var timesOf = await _repository.GetBusinessAndAchievement( id);
                if (timesOf == null)
                {
                    ViewBag.ErrorMessage = "لايوجد   بيانات";
                    return View("NotFound");
                }
            ViewBag.departmentId = timesOf.Employee.DepartmentId;
            ViewBag.employeeName = timesOf.Employee.Name;
              var modelsMapper = _mapper.Map<BusinessAndAchievementEditModelView>(timesOf);
            return View(modelsMapper);
            }


            [HttpPost]
            [ValidateAntiForgeryToken]
            [Route("Edit/{id:int}")]
            public async Task<IActionResult> Edit(int id, BusinessAndAchievementEditModelView model)
            {
                if (id != model.Id)
                {
                    ViewBag.ErrorMessage = "لايوجد   بيانات";
                    return View("NotFound");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                    var modelsMapper = _mapper.Map<BusinessAndAchievement>(model);

                    _repository.Update<BusinessAndAchievement>(modelsMapper);
                        await _repository.SavaAll();

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await _repository.GetBusinessAndAchievement(model.Id) == null)
                        {
                            ViewBag.ErrorMessage = "لايوجد   بيانات";
                            return View("NotFound");
                        }
                        else
                        {
                            throw;
                        }
                    }
                return RedirectToAction("WorksByEmployee", new { employeeId = model.EmployeeId, timesOfEvaluationAndPerformanceId = model.TimesOfEvaluationAndPerformanceId });

            }

            var timesOf = await _repository.GetBusinessAndAchievement(id);
            ViewBag.departmentId = timesOf.Employee.DepartmentId;
            ViewBag.employeeName = timesOf.Employee.Name;
            return View(model);
            }


            [HttpPost]
            [ValidateAntiForgeryToken]
            [Route("Delete/{id:int}")]
            public async Task<IActionResult> Delete(int id)
            {
                var business = await _repository.GetBusinessAndAchievement(id);
                if (business == null)
                {
                    ViewBag.ErrorMessage = "لايوجد   بيانات";
                    return View("NotFound");
                }

                try
                {
                    _repository.Delete<BusinessAndAchievement>(business);
                    await _repository.SavaAll();


                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = $"لايمكن حذف   : ( {business.TheWork} )  اذا اردت الحذف الرجاء حذف جميع الحقول المرتبط   ";
                    ViewBag.ex = e.GetBaseException();
                    return View("NotFound");


                }


            return RedirectToAction("WorksByEmployee", new { employeeId = business.EmployeeId, timesOfEvaluationAndPerformanceId = business.TimesOfEvaluationAndPerformanceId });

        }


        [HttpGet]
        [Route("AchieveAllBusiness/{employeeId:int}/{timesOfEvaluationAndPerformanceId:int}")]
        public async Task<IActionResult> AchieveAllBusiness(int employeeId, int timesOfEvaluationAndPerformanceId)
        {
            var businesses = await _repository.GetBusinessAndAchievementByEmployeeIdAndTimeOfId(employeeId, timesOfEvaluationAndPerformanceId);
            var modelsMapper = _mapper.Map<IList<AchieveAllBusinessModelView>>(businesses);
            var employee = await _repository.GetEmployee(employeeId);
            var timesOf = await _repository.GetTimesOfEvaluationAndPerformance(timesOfEvaluationAndPerformanceId);
            if (timesOf == null || employee== null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            ViewBag.departmentId = employee.Department.Id;
            ViewBag.timesOfEvaluationAndPerformanceId = timesOf.Id;
            ViewBag.employeeId = employee.Id;
            ViewBag.employeeName = employee.Name;
            return View(modelsMapper);
        }


        [HttpPost]
        [Route("AchieveAllBusiness/{employeeId:int}/{timesOfEvaluationAndPerformanceId:int}")]
        public async Task<IActionResult> AchieveAllBusiness(int employeeId, int timesOfEvaluationAndPerformanceId , IList<AchieveAllBusinessModelView> models)
        {

            var employee = await _repository.GetEmployee(employeeId);
            var timesOf = await _repository.GetTimesOfEvaluationAndPerformance(timesOfEvaluationAndPerformanceId);
            if (timesOf == null || employee == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            foreach (var modelerror in models)
            {
                if (modelerror.QuicklyPerformTheTask == 0 && modelerror.Status == 1)
                {
                    ViewBag.departmentId = employee.Department.Id;
                    ViewBag.timesOfEvaluationAndPerformanceId = timesOf.Id;
                    ViewBag.employeeId = employee.Id;
                    ViewBag.employeeName = employee.Name;
                    ViewBag.errorMessage = $"{modelerror.TheWork} سرعة العمل مطلوب في حالة انتهاء العمل ";
                    return View(models);

                }

                if (modelerror.NotesForWorkDelayed == null  && modelerror.Status > 1)
                {
                    ViewBag.departmentId = employee.Department.Id;
                    ViewBag.timesOfEvaluationAndPerformanceId = timesOf.Id;
                    ViewBag.employeeId = employee.Id;
                    ViewBag.employeeName = employee.Name;
                    ViewBag.errorMessage = $"{modelerror.TheWork}  ملاحظات العمل الملغي او المؤجل  )مطلوب في حالة تاجيل او الغاء  العمل ويجب عدم اختيار سرعة للعمل ) ";
                    return View(models);

                }

            }

            if (ModelState.IsValid)
            {
              
                var modelsMapper = _mapper.Map<IList<BusinessAndAchievement>>(models);
                foreach (var model in modelsMapper)
                {
                    if (model.Status == 1)
                    {
                        model.NotesForWorkDelayed = null;

                    }
                    else
                    {
                        model.QuicklyPerformTheTask = null;
                    }
                    _repository.Update<BusinessAndAchievement>(model);
                }
                await _repository.SavaAll();
                return RedirectToAction("WorksByEmployee", new { employeeId = employeeId, timesOfEvaluationAndPerformanceId = timesOfEvaluationAndPerformanceId });

            }
           
            ViewBag.departmentId = employee.Department.Id;
            ViewBag.timesOfEvaluationAndPerformanceId = timesOf.Id;
            ViewBag.employeeId = employee.Id;
            ViewBag.employeeName = employee.Name;
            return View(models);
        }



        [HttpGet]
        [Route("AchieveSingleBusiness/{id:int}")]
        public async Task<IActionResult> AchieveSingleBusiness(int id)
        {

            var business = await _repository.GetBusinessAndAchievement(id);
            if (business == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            var modelsMapper = _mapper.Map<AchieveAllBusinessModelView>(business);
      
            ViewBag.departmentId = business.Employee.Department.Id;
            ViewBag.timesOfEvaluationAndPerformanceId = business.TimesOfEvaluationAndPerformanceId;
            ViewBag.employeeId = business.EmployeeId;
            ViewBag.employeeName = business.Employee.Name;
            return View(modelsMapper);
        }


        [HttpPost]
        [Route("AchieveSingleBusiness/{id:int}")]
        public async Task<IActionResult> AchieveSingleBusiness(int id, 
            AchieveAllBusinessModelView model)
        {

            if (id != model.Id)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            var modelsMapper = _mapper.Map<BusinessAndAchievement>(model);

            if (ModelState.IsValid)
            {
                if (model.QuicklyPerformTheTask == 0 && model.Status == 1)
                {
                    ViewBag.departmentId = modelsMapper.Employee.Department.Id;
                    ViewBag.timesOfEvaluationAndPerformanceId = modelsMapper.TimesOfEvaluationAndPerformanceId;
                    ViewBag.employeeId = modelsMapper.EmployeeId;
                    ViewBag.employeeName = modelsMapper.Employee.Name;
                    ViewBag.errorMessage = $"{model.TheWork} سرعة العمل مطلوب في حالة انتهاء العمل ";
                    return View(model);

                }

                if (model.NotesForWorkDelayed == null && model.Status > 1)
                {
                    ViewBag.departmentId = modelsMapper.Employee.Department.Id;
                    ViewBag.timesOfEvaluationAndPerformanceId = modelsMapper.TimesOfEvaluationAndPerformanceId;
                    ViewBag.employeeId = modelsMapper.EmployeeId;
                    ViewBag.employeeName = modelsMapper.Employee.Name;
                    ViewBag.errorMessage = $"{model.TheWork}  ملاحظات العمل الملغي او المؤجل  )مطلوب في حالة تاجيل او الغاء  العمل ويجب عدم اختيار سرعة للعمل ) ";
                    return View(model);

                }
                
              
                    if (model.Status == 1)
                    {
                      modelsMapper.NotesForWorkDelayed = null;

                    }
                    else
                    {
                      modelsMapper.QuicklyPerformTheTask = null;
                    }
                    _repository.Update<BusinessAndAchievement>(modelsMapper);
              
                await _repository.SavaAll();
                return RedirectToAction("WorksByEmployee", new { employeeId = model.EmployeeId, timesOfEvaluationAndPerformanceId = model.TimesOfEvaluationAndPerformanceId });
            }
            ViewBag.departmentId = modelsMapper.Employee.Department.Id;
            ViewBag.timesOfEvaluationAndPerformanceId = modelsMapper.TimesOfEvaluationAndPerformanceId;
            ViewBag.employeeId = modelsMapper.EmployeeId;
            ViewBag.employeeName = modelsMapper.Employee.Name;
            return View(model);
        }

    }

}
