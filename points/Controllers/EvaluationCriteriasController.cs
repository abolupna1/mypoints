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
    [Route("EvaluationCriterias")]
    [Authorize(Roles = "Admin")]
    public class EvaluationCriteriasController : Controller
    {
        private readonly IPointsRepository _repository;

        public EvaluationCriteriasController(IPointsRepository repository)
        {
            _repository = repository;
        }



        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetEvaluationCriterias());
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create( EvaluationCriteria evaluationCriteria)
        {
            if (ModelState.IsValid)
            {
                _repository.Add<EvaluationCriteria>(evaluationCriteria);
                await _repository.SavaAll();
                return RedirectToAction(nameof(Index));
            }
            return View(evaluationCriteria);
        }

        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {


            var evaluationCriteria = await _repository.GetEvaluationCriteria(id);
            if (evaluationCriteria == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            return View(evaluationCriteria);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id,  EvaluationCriteria evaluationCriteria)
        {
            if (id != evaluationCriteria.Id)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update<EvaluationCriteria>(evaluationCriteria);
                    await _repository.SavaAll();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_repository.GetEvaluationCriteria(evaluationCriteria.Id) == null)
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
            return View(evaluationCriteria);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var evaluationCriteria = await _repository.GetEvaluationCriteria(id);
            if (evaluationCriteria == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            try
            {
                _repository.Delete<EvaluationCriteria>(evaluationCriteria);
                await _repository.SavaAll();


            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"لايمكن حذف   : ( {evaluationCriteria.CriteriaName} )  اذا اردت الحذف الرجاء حذف جميع   التقييات المرتبط بهذا المعيار";
               ViewBag.error = ex.Message;
                return View("NotFound");


            }

            return RedirectToAction(nameof(Index));
        }


    }
}
