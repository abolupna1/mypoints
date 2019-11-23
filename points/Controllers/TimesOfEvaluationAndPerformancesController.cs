using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using points.Data;
using points.Data.Repositories;
using points.Models;
using points.ModelViews.Reports;

namespace points.Controllers
{
    [Route("TimesOfEvaluationAndPerformances")]
   
    public class TimesOfEvaluationAndPerformancesController : Controller
    {
        private readonly IPointsRepository _repository;

        public TimesOfEvaluationAndPerformancesController(IPointsRepository repository)
        {
            _repository = repository;
        }


        



        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetTimesOfEvaluationAndPerformances());
        }


        [Route("Departments/{timesOfEvaluationAndPerformanceId:int}")]
        public async Task<IActionResult> Departments(int timesOfEvaluationAndPerformanceId)
        {
            var timesOf = await _repository.GetTimesOfEvaluationAndPerformance(timesOfEvaluationAndPerformanceId);
            if (timesOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var departments = await _repository.GetAppUserDepartmentsByUserId(userId);
            ViewBag.timesOfEvaluationAndPerformanceId = timesOfEvaluationAndPerformanceId;
            return View(departments);
        }

        [Route("CycleReport/{id:int}")]

        [Authorize(Roles = "Dean,Admin")]
        public async Task<IActionResult> CycleReport(int id)
        {

            var timesOf = await _repository.GetTimesOfEvaluationAndPerformance(id);
            if (timesOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            var model = new CycleReportModelView
            {
                Cyckle = timesOf,
                Employees=await _repository.GetEmployees()
            };
            return View(model);
        }
        [Route("ReportByDepartment/{departmentId:int}/{timeOfId:int}")]
        public async Task<IActionResult> ReportByDepartment(int departmentId,int timeOfId)
        {
            var timesOf = await _repository.GetTimesOfEvaluationAndPerformance(timeOfId);
            var department = await _repository.GetDepartment(departmentId);
            if (timesOf == null || department==null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            
             

            var model = new DepartmentReportModelView
            {
                Name = department.Name,
                BusinessAndAchievements= await _repository.GetDepartmentBusinessInThisSicle(department.Id, timesOf.Id),
                Occasions= await _repository.GetDepartmentOccasionsInThisSicle(department.Id, timesOf.Id),
                Evaluations = await _repository.GetDepartmentEvaluationsInThisSicle(department.Id, timesOf.Id),
                Courses = await _repository.GetDepartmentCoursesInThisSicle(department.Id, timesOf.Id),
                
            };
            ViewBag.departmentId = department.Id;
            ViewBag.timesOfEvaluationAndPerformanceId = timesOf.Id;
            ViewBag.departmentName = department.Name;



            return View(model);
        }
      
        [Route("Create")]
        [Authorize(Roles = "Dean,Admin")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        [Authorize(Roles = "Dean,Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,FromDate,ToDate,Status")] TimesOfEvaluationAndPerformance timesOf)
        {
            if (ModelState.IsValid)
            {
                _repository.Add<TimesOfEvaluationAndPerformance>(timesOf);
                await _repository.SavaAll();
                return RedirectToAction(nameof(Index));
            }
            return View(timesOf);
        }

        [Route("Edit/{id:int}")]
        [Authorize(Roles = "Dean,Admin")]
        public async Task<IActionResult> Edit(int id)
        {


            var timesOf = await _repository.GetTimesOfEvaluationAndPerformance(id);
            if (timesOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
     
            return View(timesOf);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int}")]
        [Authorize(Roles = "Dean,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,FromDate,ToDate,Status")]
        TimesOfEvaluationAndPerformance timesOf)
        {
            if (id != timesOf.Id)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update<TimesOfEvaluationAndPerformance>(timesOf);
                    await _repository.SavaAll();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_repository.GetTimesOfEvaluationAndPerformance(timesOf.Id) == null)
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

            return View(timesOf);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int}")]
        [Authorize(Roles = "Dean,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var timesOf = await _repository.GetTimesOfEvaluationAndPerformance(id);
            if (timesOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            try
            {
                _repository.Delete<TimesOfEvaluationAndPerformance>(timesOf);
                await _repository.SavaAll();


            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = $"لايمكن حذف   : ( {timesOf.Title} )  اذا اردت الحذف الرجاء حذف جميع الحقول المرتبط   ";
                ViewBag.ex = e.GetBaseException();
                return View("NotFound");


            }


            return RedirectToAction(nameof(Index));
        }


    }
}
