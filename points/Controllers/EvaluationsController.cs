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
    [Route("Evaluations")]

    public class EvaluationsController : Controller
    {
        private readonly IPointsRepository _repository;

        public EvaluationsController(IPointsRepository repository)
        {
            _repository = repository;
        }

        [Route("EmployeeEvaluationsInThisSicle/{employeeId:int}/{timeOfId:int}")]
        public async Task<IActionResult> EmployeeEvaluationsInThisSicle(int employeeId, int timeOfId)
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
            return View(await _repository.GetEmployeeEvaluationsInThisSicle(employeeId, timeOfId));
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
            ViewData["evaluationCriteria"] = await _repository.GetEvaluationCriteriasByJopType(emp.JopType);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/{employeeId:int}/{timeOfId:int}")]
        public async Task<IActionResult> Create(int employeeId, int timeOfId,IList<Evaluation> evaluations)
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
                foreach (var evaluation in evaluations)
                {
                    _repository.Add<Evaluation>(evaluation);

                }
                await _repository.SavaAll();
                return RedirectToAction("EmployeeEvaluationsInThisSicle", new { employeeId = employeeId, timeOfId = timeOfId });
            }
            ViewBag.departmentId = emp.DepartmentId;
            ViewBag.timesOfEvaluationAndPerformanceId = timeOfId;
            ViewBag.employeeId = emp.Id;
            ViewBag.employeeName = emp.Name;
            ViewData["evaluationCriteria"] = await _repository.GetEvaluationCriteriasByJopType(emp.JopType);
            return View();
        }

        [Route("Edit/{employeeId:int}/{timeOfId:int}")]
        public async Task<IActionResult> Edit(int employeeId,int timeOfId)
        {
            var evaluations = await _repository.GetEmployeeEvaluationsInThisSicle(employeeId, timeOfId); ;
            var emp = await _repository.GetEmployee(employeeId);
            var timeOf = await _repository.GetTimesOfEvaluationAndPerformance(timeOfId);
            if ( emp == null || timeOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            ViewBag.departmentId = emp.DepartmentId;
            ViewBag.timesOfEvaluationAndPerformanceId = timeOf.Id;
            ViewBag.employeeId = emp.Id;
            ViewBag.employeeName = emp.Name;
            return View(evaluations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{employeeId:int}/{timeOfId:int}")]
        public async Task<IActionResult> Edit(int employeeId, int timeOfId,IList<Evaluation> evaluations)
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
                foreach (var evaluation in evaluations)
                {
                    _repository.Update<Evaluation>(evaluation);

                }
                await _repository.SavaAll();
                return RedirectToAction("EmployeeEvaluationsInThisSicle", new { employeeId = employeeId, timeOfId = timeOfId });
            }
            ViewBag.departmentId = emp.DepartmentId;
            ViewBag.timesOfEvaluationAndPerformanceId = timeOf.Id;
            ViewBag.employeeId = emp.Id;
            ViewBag.employeeName = emp.Name;
            return View(evaluations);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{employeeId:int}/{timeOfId:int}")]
        public async Task<IActionResult> Delete(int employeeId,int timeOfId)
        {
            var emp = await _repository.GetEmployee(employeeId);
            var timeOf = await _repository.GetTimesOfEvaluationAndPerformance(timeOfId);
            if (emp == null || timeOf == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            var evaluations = await _repository.GetEmployeeEvaluationsInThisSicle(employeeId, timeOfId); ;

            try
            {
                foreach (var evaluation in evaluations)
                {
                    _repository.Delete<Evaluation>(evaluation);

                }
                await _repository.SavaAll();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"لايمكن حذف   :  ";
                ViewBag.ex = ex.GetBaseException();
                return View("NotFound");


            }

            return RedirectToAction("EmployeeEvaluationsInThisSicle", new { employeeId = employeeId, timeOfId = timeOfId });

        }


    }
}
