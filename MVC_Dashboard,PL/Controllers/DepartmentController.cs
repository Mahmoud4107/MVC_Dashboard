using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC_Dashboard.BLL.Interfaces;
using MVC_Dashboard.DAL.Models;
using System;
namespace MVC_Dashboard_PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IUnitOfWork unit,IWebHostEnvironment env)
        {
            _unit = unit;
            _env = env;
        }
        public IActionResult Index()
        {
            var departments = _unit.Repository<Department>().GetAll();

            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
              if(ModelState.IsValid) // Server Side Validation 
              {
                _unit.Repository<Department>().Add(department);

                int count = _unit.Complete();
                if (count > 0)
                    return RedirectToAction(nameof(Index));
              }

            return View(department);
        }

        public IActionResult Details(int? id,string ViewName = "Details")
        {
            if (id == null)
                return BadRequest(); // 400

            var department = _unit.Repository<Department>().GetById(id.Value);

            if (department == null)
                return NotFound();  //  404

            return View(ViewName, department);
        }

        //Department/Edit/10
        public IActionResult Edit(int? id)
        {
            ///if(id == null)
            ///    return BadRequest();
            ///var department = Repository.GetById(id.Value);
            ///if (department == null)
            ///    return NotFound();
            ///return View(department);

            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id ,Department department)
        {
            if(id != department.Id)
                return BadRequest();

            if(!ModelState.IsValid)
                return View(department);
            try
            {
                _unit.Repository<Department>().Update(department);
                
                int Count = _unit.Complete();
                if (Count > 0)
                    return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log Exception
                // Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occurred During Updating This Department");

                return View(department);
            }
            return View(department);

        }

        //Department/Delete/10
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            ///if(id is null)
            ///    return BadRequest();
            ///var department = Repository.GetById(id.Value);
            ///if (department == null)
            ///    return NotFound();
            ///return View(department);

            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute]int id,Department department)
        {
            if(id != department.Id)
                return BadRequest();
            try
            {
                _unit.Repository<Department>().Delete(department);

                int Count = _unit.Complete();
                if (Count > 0)
                    return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log Exception
                // Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty,ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occurred During Deleting This Department");

                return View("department");

            }
            return View(department);
        }





    }
}
