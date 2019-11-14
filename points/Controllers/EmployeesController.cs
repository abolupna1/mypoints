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
    [Route("Employees")]
    [Authorize(Roles = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly IPointsRepository _repository;

        public EmployeesController(IPointsRepository repository)
        {
            _repository = repository;
        }





        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetEmployees());
        }

        [Route("Create")]
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("Id,Name,EmployeeNo,Mobile,JopType,JopName,DepartmentId,SectionId,UnitId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _repository.Add<Employee>(employee);
                await _repository.SavaAll();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", employee.DepartmentId);
            return View(employee);
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
            ViewData["DepartmentId"] = new SelectList(_repository.GetDepartments().Result, "Id", "Name", section.DepartmentId);


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
            catch (DbUpdateConcurrencyException)
            {
                ViewBag.ErrorMessage = "توجد حقول مرتبطة بهذا القسم / الرجاء حذف جميع الحقول المرتبطة من ثم  يمكن حذف القسم";
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
