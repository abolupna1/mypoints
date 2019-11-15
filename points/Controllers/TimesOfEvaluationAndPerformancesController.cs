using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using points.Data;
using points.Data.Repositories;
using points.Models;

namespace points.Controllers
{
    [Route("TimesOfEvaluationAndPerformances")]
    [Authorize(Roles = "Admin")]
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

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
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
