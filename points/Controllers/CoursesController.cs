using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using points.Data;
using points.Data.Repositories;
using points.Models;

namespace points.Controllers
{
    [Route("Courses")]
    public class CoursesController : Controller
    {

        private readonly IPointsRepository _repository;

        public CoursesController(IPointsRepository repository)
        {
            _repository = repository;
        }

        [Route("EmployeeCoursesInThisSicle/{employeeId:int}/{timeOfId:int}")]
         public async Task<IActionResult> EmployeeCoursesInThisSicle(int employeeId,int timeOfId)
        {
            var emp = await _repository.GetEmployee(employeeId);
            var timeOf = await _repository.GetTimesOfEvaluationAndPerformance(timeOfId);
            if (emp == null || timeOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
         
            ViewBag.sycleStatus = timeOf.Status;
            ViewBag.departmentId = emp.DepartmentId;
            ViewBag.timesOfEvaluationAndPerformanceId = timeOfId;
            ViewBag.employeeId = emp.Id;
            ViewBag.employeeName = emp.Name;
            return View(await _repository.GetEmployeeCoursesInThisSicle(employeeId,timeOfId));
        }

        [Route("Create/{employeeId:int}/{timeOfId:int}")]
        public async Task<IActionResult> Create(int employeeId, int timeOfId) 
        {
            var emp = await _repository.GetEmployee(employeeId);
            var timeOf = await _repository.GetTimesOfEvaluationAndPerformance(timeOfId);
            if (emp == null || timeOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            ViewBag.departmentId = emp.DepartmentId;
            ViewBag.timesOfEvaluationAndPerformanceId = timeOfId;
            ViewBag.employeeId = emp.Id;
            ViewBag.employeeName = emp.Name;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/{employeeId:int}/{timeOfId:int}")]
        public async Task<IActionResult> Create(int employeeId, int timeOfId,Course course)
        {
            var emp = await _repository.GetEmployee(employeeId);
            var timeOf = await _repository.GetTimesOfEvaluationAndPerformance(timeOfId);
            if (emp == null || timeOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            if (ModelState.IsValid)
            {
                _repository.Add<Course>(course);
                await _repository.SavaAll();
                return RedirectToAction("EmployeeCoursesInThisSicle", new { employeeId = employeeId, timeOfId = timeOfId });
            }
            ViewBag.departmentId = emp.DepartmentId;
            ViewBag.timesOfEvaluationAndPerformanceId = timeOfId;
            ViewBag.employeeId = emp.Id;
            ViewBag.employeeName = emp.Name;
            return View();
        }



        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _repository.GetCourse(id);
            var emp = await _repository.GetEmployee(course.EmployeeId);
            var timeOf = await _repository.GetTimesOfEvaluationAndPerformance(course.TimesOfEvaluationAndPerformanceId);
            if (course == null || emp == null || timeOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            ViewBag.departmentId = emp.DepartmentId;
            ViewBag.timesOfEvaluationAndPerformanceId = timeOf.Id;
            ViewBag.employeeId = emp.Id;
            ViewBag.employeeName = emp.Name;
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id,Course course)
        {
           
            var emp = await _repository.GetEmployee(course.EmployeeId);
            var timeOf = await _repository.GetTimesOfEvaluationAndPerformance(course.TimesOfEvaluationAndPerformanceId);
            if ( emp == null || timeOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            if (ModelState.IsValid)
            {
                _repository.Update<Course>(course);
                await _repository.SavaAll();
                return RedirectToAction("EmployeeCoursesInThisSicle", new { employeeId = course.EmployeeId, timeOfId = course.TimesOfEvaluationAndPerformanceId });

            }
            ViewBag.departmentId = emp.DepartmentId;
            ViewBag.timesOfEvaluationAndPerformanceId = timeOf.Id;
            ViewBag.employeeId = emp.Id;
            ViewBag.employeeName = emp.Name;
            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _repository.GetCourse (id);
            if (course == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            try
            {
                _repository.Delete<Course>(course);
                await _repository.SavaAll();


            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"لايمكن حذف   : ( {course.Title}  ";
                ViewBag.ex = ex.GetBaseException();
                return View("NotFound");


            }

            return RedirectToAction("EmployeeCoursesInThisSicle", new { employeeId = course.EmployeeId, timeOfId = course.TimesOfEvaluationAndPerformanceId });

        }


    }
}
