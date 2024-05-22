using BLL.Interfaces;
using DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext context;

		public IDepartmentRepository DepartmentRepository { get; set; }
		public IEmployeeRepository EmployeeRepository { get; set; }

        public UnitOfWork(AppDbContext context)
        {
			this.context = context;
			EmployeeRepository = new EmployeeRepository(context);
            DepartmentRepository = new DepartmentRepository(context);
		}

		public int Complete()
		{
			return context.SaveChanges();
		}
	}
}
