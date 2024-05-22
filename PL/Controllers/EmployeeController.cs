using AutoMapper;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PL.Helper;
using PL.Models;
using System.Collections.Generic;

namespace PL.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
			this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public IActionResult Index(string SearchValue = "")
		{
			IEnumerable<Employee> employees;
			IEnumerable<EmployeeViewModel> employeesViewModel;

            if (string.IsNullOrEmpty(SearchValue))
			{
                employees = unitOfWork.EmployeeRepository.GetAll();
				employeesViewModel = mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
			}
			else
			{
                employees = unitOfWork.EmployeeRepository.Search(SearchValue);
                employeesViewModel = mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            }

            return View(employeesViewModel);
		}
		public IActionResult Create()
		{
			ViewBag.Departments =unitOfWork.DepartmentRepository.GetAll();
			return View(new EmployeeViewModel());
		}

		[HttpPost]
        public IActionResult Create(EmployeeViewModel employeeViewModel)
        {
			//ModelState["Department"].ValidationState = ModelValidationState.Valid;
			if (ModelState.IsValid)
			{
				//// Manual Mapping
				//Employee employee = new Employee
				//{
				//	Name = employeeViewModel.Name,
				//	Address = employeeViewModel.Address,
				//	Email = employeeViewModel.Email,
				//	Salary = employeeViewModel.Salary,
				//	HireDate = employeeViewModel.HireDate,
				//	IsActive = employeeViewModel.IsActive,
				//	DepartmentId = employeeViewModel.DepartmentId,
				//};

				// AutoMapper
				var employee = mapper.Map<Employee>(employeeViewModel);
				if (employeeViewModel.Image != null)
					employee.ImageUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Images");

				unitOfWork.EmployeeRepository.Add(employee);
				unitOfWork.Complete();
				return RedirectToAction(nameof(Index));
			}

			ViewBag.Departments = unitOfWork.DepartmentRepository.GetAll();

			return View(employeeViewModel);
		}

		public IActionResult Details(int? id)
		{
			if (id is null)
				return BadRequest();

			var employee = unitOfWork.EmployeeRepository.GetById(id);
			var employeeViewModel = mapper.Map<EmployeeViewModel>(employee);


            if (employee is null)
				return NotFound();

			return View(employeeViewModel);
		}

		[HttpGet]
		public IActionResult Update(int? id)
		{
			if (id is null)
				return BadRequest();

			var employee = unitOfWork.EmployeeRepository.GetById(id);
            var employeeViewModel = mapper.Map<EmployeeViewModel>(employee);
			employeeViewModel.ImageUrl = employee.ImageUrl;

            if (employee is null)
				return NotFound();

			ViewBag.Departments = unitOfWork.DepartmentRepository.GetAll();

			return View(employeeViewModel);
		}

		[HttpPost]
		public IActionResult Update(EmployeeViewModel employeeViewModel)
		{
            var employee = mapper.Map<Employee>(employeeViewModel);
			if(employeeViewModel.Image != null)
				employee.ImageUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Images");

            unitOfWork.EmployeeRepository.Update(employee);
			unitOfWork.Complete();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int? id)
		{
			if (id is null)
				return BadRequest();

			var employee = unitOfWork.EmployeeRepository.GetById(id);
			if(employee.ImageUrl != null)
				DocumentSettings.DeleteFile(employee.ImageUrl, "Images");

			if (employee is null)
				return NotFound();

			unitOfWork.EmployeeRepository.Delete(employee);
			unitOfWork.Complete();
			return RedirectToAction(nameof(Index));
		}
	}
}
