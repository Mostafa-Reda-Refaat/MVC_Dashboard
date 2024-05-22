using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
	public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
	{
		private readonly AppDbContext context;

		public EmployeeRepository(AppDbContext context) : base(context)
        {
			this.context = context;
		}

		public IEnumerable<Employee> GetEmployeesByDepartmentName(string departmentName)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Employee> Search(string name)
		{
			var result = context.Employees.Where(emp => emp.Name.Trim().ToLower().Contains(name.Trim().ToLower()) ||
														emp.Email.Trim().ToLower().Contains(name.Trim().ToLower()));
			return result;
		}

		//public int Add(Employee employee)
		//{
		//	context.Employees.Add(employee);
		//	return context.SaveChanges();
		//}

		//public int Delete(Employee employee)
		//{
		//	context.Employees.Remove(employee);
		//	return context.SaveChanges();
		//}

		//public IEnumerable<Employee> GetAll()
		//	=> context.Employees.ToList();

		//public Employee GetById(int id)
		//	=> context.Employees.FirstOrDefault(x => x.Id == id);

		//public int Update(Employee employee)
		//{
		//	context.Employees.Update(employee);
		//	return context.SaveChanges();
		//}
	}
}
