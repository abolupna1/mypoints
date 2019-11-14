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
    [Route("Units")]
    [Authorize(Roles = "Admin")]
    public class UnitsController : Controller
    {
        private readonly IPointsRepository _repository;

        public UnitsController(IPointsRepository repository)
        {
            _repository = repository;
        }



        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetUnits());
        }

        [Route("Create")]
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name");
            ViewData["SectionId"] = new SelectList(_repository.GetSections().Result, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("Id,Name,DepartmentId,SectionId")] Unit unit)
        {
            if (ModelState.IsValid)
            {
                _repository.Add<Unit>(unit);
                await _repository.SavaAll();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", unit.DepartmentId);
            return View(unit);
        }

        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {


            var unit = await _repository.GetUnit(id);
            if (unit == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", unit.DepartmentId);
            ViewData["SectionId"] = new SelectList(_repository.GetSections().Result, "Id", "Name", unit.SectionId);

            return View(unit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DepartmentId,SectionId")] Unit unit)
        {
            if (id != unit.Id)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update<Unit>(unit);
                    await _repository.SavaAll();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_repository.GetUnit(unit.Id) == null)
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
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", unit.DepartmentId);
            ViewData["SectionId"] = new SelectList(_repository.GetSections().Result, "Id", "Name", unit.SectionId);

            return View(unit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var unit = await _repository.GetUnit(id);
            if (unit == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            try
            {
                _repository.Delete<Unit>(unit);
                await _repository.SavaAll();


            }
            catch (DbUpdateConcurrencyException)
            {
                ViewBag.ErrorMessage = "توجد حقول مرتبطة الرجاء حذفها من ثم حاول حذف الوحدة";
                return View("NotFound");


            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetSectionsByDepartmentId/{departmentId:int}")]
        public async Task<IEnumerable<Section>> GetSectionsByDepartmentId(int departmentId)
        {
            return await _repository.GetSectionsByDepartmentId(departmentId);
        }
    }
}
