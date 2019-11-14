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
    [Route("Sections")]
    [Authorize(Roles = "Admin")]
    public class SectionsController : Controller
    {
        private readonly IPointsRepository _repository;

        public SectionsController(IPointsRepository repository)
        {
            _repository = repository;
        }



        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetSections());
        }

        [Route("Create")]
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList( _repository.GetDepartments().Result, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("Id,Name,DepartmentId")] Section section)
        {
            if (ModelState.IsValid)
            {
                _repository.Add<Section>(section);
                await _repository.SavaAll();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name",section.DepartmentId);
            return View(section);
        }

        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {


            var section = await _repository.GetSection(id);
            if (section == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name",section.DepartmentId);

            return View(section);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DepartmentId")] Section section)
        {
            if (id != section.Id)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update<Section>(section);
                    await _repository.SavaAll();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_repository.GetSection(section.Id) == null)
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
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", section.Id);

            return View(section);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var section = await _repository.GetSection(id);
            if (section == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            try
            {
                _repository.Delete<Section>(section);
                await _repository.SavaAll();


            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = $"لايمكن حذف   : ( {section.Name} )  اذا اردت الحذف الرجاء حذف جميع الوحدات والموظفين المرتبطين بهذا القسم ";
                ViewBag.ex = e.GetBaseException();
                return View("NotFound");


            }


            return RedirectToAction(nameof(Index));
        }


    
    }
}
