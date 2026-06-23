
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Extensions.Hosting;
using MVC_Dashboard.BLL.Interfaces;
using MVC_Dashboard.BLL.Repositories;
using MVC_Dashboard.DAL.Models;
using System;
using System.Linq;

namespace MVC_Dashboard_PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repository;
        public IWebHostEnvironment _env { get; }

        public EmployeeController(IEmployeeRepository repository, IWebHostEnvironment env) // ASK CLR to create object from EmployeeRepository
        {
            _repository = repository;
            _env = env;
        }

        // Employee/index
        public IActionResult Index(string searchInput)
        {
            var Employees = Enumerable.Empty<Employee>();
            if(searchInput is null)
            {
                Employees = _repository.GetAll();
            }
                
            else
                 Employees = _repository.SearchByName(searchInput.ToLower());
            return View(Employees);
        }

        // Employee/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int Count = _repository.Add(employee);

                    if (Count > 0)
                        return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                // Log Exception 
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occurred During Creating This Employee"); // Friendly Message
            }
            return View(employee);

        }
        // Employee/Details/id?
        [HttpGet]
        public IActionResult Details(int? id, string ActionName = "Details")
        {
            if (id == null)
                return BadRequest();

            var employee = _repository.GetById(id.Value);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // Employee/Edit/id?
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();
            try
            {
                if (ModelState.IsValid)
                {
                    var Count = _repository.Update(employee);
                    if (Count > 0)
                        return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                // Log Exception 
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occurred During Updating This Employee"); // Friendly Message
            }
            return View(employee);
        }

        //Employee/Delete/id?
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete([FromRoute] int? id,Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();
            try
            {
                var Count = _repository.Delete(employee);
                if (Count > 0)
                    return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                // Log Exception 
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occurred During Updating This Employee"); // Friendly Message
            }
            return View(employee);
        }
    }
}
