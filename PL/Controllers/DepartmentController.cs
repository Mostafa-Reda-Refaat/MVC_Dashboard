using AutoMapper;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PL.Models;
using System.Collections.Generic;

namespace PL.Controllers
{
	public class DepartmentController : Controller
	{
		private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        //private readonly IDepartmentRepository departmentRepository;

        public DepartmentController(/*IDepartmentRepository departmentRepository*/ IUnitOfWork unitOfWork, IMapper mapper)
        {
			//this.departmentRepository = departmentRepository;
			this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public IActionResult Index()
		{
			var departments = unitOfWork.DepartmentRepository.GetAll();
			IEnumerable<DepartmentViewModel> departmentViewModels = mapper.Map<IEnumerable<DepartmentViewModel>>(departments);


            return View(departmentViewModels);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View(new DepartmentViewModel());
		}

		[HttpPost]
		public IActionResult Create(DepartmentViewModel departmentViewModel)
		{
			var department = mapper.Map<Department>(departmentViewModel);
            unitOfWork.DepartmentRepository.Add(department);
			unitOfWork.Complete();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Details(int? id)
		{
			if (id is null)
				return BadRequest();

			var department = unitOfWork.DepartmentRepository.GetById(id);
			var departmentViewModels = mapper.Map<DepartmentViewModel>(department);

			if (department is null)
				return NotFound();

			return View(departmentViewModels);
		}

		[HttpGet]
		public IActionResult Update(int? id)
		{
			if (id is null)
				return BadRequest();

			var department = unitOfWork.DepartmentRepository.GetById(id);
			var departmentViewModels = mapper.Map<DepartmentViewModel>(department);

			if (department is null)
				return NotFound();

			return View(departmentViewModels);
		}

		[HttpPost]
		public IActionResult Update(DepartmentViewModel departmentViewModel)
		{
			var department = mapper.Map<Department>(departmentViewModel);

			unitOfWork.DepartmentRepository.Update(department);
			unitOfWork.Complete();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int? id)
		{
			if (id is null)
				return BadRequest();

			var department = unitOfWork.DepartmentRepository.GetById(id);

			if (department is null)
				return NotFound();

			unitOfWork.DepartmentRepository.Delete(department);
			unitOfWork.Complete();
			return RedirectToAction(nameof(Index));
		}
	}
}
