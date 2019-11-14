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
    [Route("Departments")]
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : Controller
    {
        private readonly IPointsRepository _repository;

        public DepartmentsController(IPointsRepository repository)
        {
            _repository = repository;
        }

       
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetDepartments());
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                _repository.Add<Department>(department);
               await _repository.SavaAll();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
           

            var department = await _repository.GetDepartment(id);
            if (department == null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }
         
            return View(department);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
        {
            if (id != department.Id)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update<Department>(department);
                    await _repository.SavaAll();
                  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if ( _repository.GetDepartment(department.Id) == null)
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
            return View(department);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _repository.GetDepartment(id);
            if (department==null)
            {
                ViewBag.ErrorMessage = "لايوجد   بيانات";
                return View("NotFound");
            }

            try
            {
                _repository.Delete<Department>(department);
                await _repository.SavaAll();


            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"لايمكن حذف   : ( {department.Name} )  اذا اردت الحذف الرجاء حذف جميع  الاقسام والوحدات والموظفين المرتبطين بهذا القسم ";
                ViewBag.ex = ex.GetBaseException();
                return View("NotFound");


            }
       
            return RedirectToAction(nameof(Index));
        }

     
    }
}
