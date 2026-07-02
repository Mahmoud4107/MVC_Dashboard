using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Extensions.Hosting;
using MVC_Dashboard.BLL.Interfaces;
using MVC_Dashboard.BLL.Repositories;
using MVC_Dashboard.DAL.Models;
using MVC_Dashboard_PL.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC_Dashboard_PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        public IUnitOfWork _unit { get; }
        public IWebHostEnvironment _env { get; }

        public EmployeeController(IMapper mapper,IUnitOfWork unit, IWebHostEnvironment env) // ASK CLR to create object from EmployeeRepository
        {
            _mapper = mapper;
            _unit = unit;
            _env = env;
        }

        // Employee/index
        public IActionResult Index(string searchInput)
        {
            var Employees = Enumerable.Empty<Employee>();
            if(searchInput is null)
                Employees = _unit.Repository<Employee>().GetAll();
                
            else
            {
                var Emplyee = _unit.Repository<Employee>() as EmployeeRepository;
                Employees = Emplyee.SearchByName(searchInput.ToLower());
            }

            var MappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);

            return View(MappedEmp);
        }

        // Employee/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel employeeViwModel)
        {
            var MappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeViwModel);
            try
            {
                if (ModelState.IsValid)
                {
                     _unit.Repository<Employee>().Add(MappedEmp);
                    int Count = _unit.Complete();

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
            return View(MappedEmp);

        }
        // Employee/Details/id?
        [HttpGet]
        public IActionResult Details(int? id, string ActionName = "Details")
        {
            if (id == null)
                return BadRequest();

            var employee = _unit.Repository<Employee>().GetById(id.Value);

            var MappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (MappedEmp == null)
                return NotFound();

            return View(MappedEmp);
        }

        // Employee/Edit/id?
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, EmployeeViewModel employeeViewModel)
        {
            if (id != employeeViewModel.Id)
                return BadRequest();

            var MappedEmp = _mapper.Map<EmployeeViewModel,Employee>(employeeViewModel);

            try
            {
                if (ModelState.IsValid)
                {
                    _unit.Repository<Employee>().Update(MappedEmp);
                    int Count = _unit.Complete();
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
            return View(MappedEmp);
        }

        //Employee/Delete/id?
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete([FromRoute] int? id,EmployeeViewModel employeeViewModel)
        {
            if (id != employeeViewModel.Id)
                return BadRequest();

            var MappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeViewModel);
            try
            {
                _unit.Repository<Employee>().Delete(MappedEmp);
                int Count = _unit.Complete();
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
            return View(MappedEmp);
        }
    }
}
