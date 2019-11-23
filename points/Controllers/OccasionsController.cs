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
    [Route("Occasions")]
    public class OccasionsController : Controller
    {

        private readonly IPointsRepository _repository;

        public OccasionsController(IPointsRepository repository)
        {
            _repository = repository;
        }

        [Route("EmployeeOccasionsInThisSicle/{employeeId:int}/{timeOfId:int}")]
        public async Task<IActionResult> EmployeeOccasionsInThisSicle(int employeeId, int timeOfId)
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
            return View(await _repository.GetEmployeeOccasionsInThisSicle(employeeId, timeOfId));
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
        public async Task<IActionResult> Create(int employeeId, int timeOfId, Occasion occasion)
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
                _repository.Add<Occasion>(occasion);
                await _repository.SavaAll();
                return RedirectToAction("EmployeeOccasionsInThisSicle", new { employeeId = employeeId, timeOfId = timeOfId });
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
            var occasion = await _repository.GetOccasion(id);
            var emp = await _repository.GetEmployee(occasion.EmployeeId);
            var timeOf = await _repository.GetTimesOfEvaluationAndPerformance(occasion.TimesOfEvaluationAndPerformanceId);
            if (occasion == null || emp == null || timeOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            ViewBag.departmentId = emp.DepartmentId;
            ViewBag.timesOfEvaluationAndPerformanceId = timeOf.Id;
            ViewBag.employeeId = emp.Id;
            ViewBag.employeeName = emp.Name;
            return View(occasion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, Occasion occasion)
        {

            var emp = await _repository.GetEmployee(occasion.EmployeeId);
            var timeOf = await _repository.GetTimesOfEvaluationAndPerformance(occasion.TimesOfEvaluationAndPerformanceId);
            if (emp == null || timeOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            if (ModelState.IsValid)
            {
                _repository.Update<Occasion>(occasion);
                await _repository.SavaAll();
                return RedirectToAction("EmployeeOccasionsInThisSicle", new { employeeId = occasion.EmployeeId, timeOfId = occasion.TimesOfEvaluationAndPerformanceId });

            }
            ViewBag.departmentId = emp.DepartmentId;
            ViewBag.timesOfEvaluationAndPerformanceId = timeOf.Id;
            ViewBag.employeeId = emp.Id;
            ViewBag.employeeName = emp.Name;
            return View(occasion);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var occasion = await _repository.GetOccasion(id);
            if (occasion == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            try
            {
                _repository.Delete<Occasion>(occasion);
                await _repository.SavaAll();


            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"لايمكن حذف   : ( {occasion.Title}  ";
                ViewBag.ex = ex.GetBaseException();
                return View("NotFound");


            }

            return RedirectToAction("EmployeeOccasionsInThisSicle", new { employeeId = occasion.EmployeeId, timeOfId = occasion.TimesOfEvaluationAndPerformanceId });

        }

    }
}
