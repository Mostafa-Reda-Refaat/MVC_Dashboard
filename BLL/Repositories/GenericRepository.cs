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
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly AppDbContext context;

		public GenericRepository(AppDbContext context)
		{
			this.context = context;
		}
		public void Add(T entity)
		{
			context.Set<T>().Add(entity);
			//return context.SaveChanges();
		}

		public void Delete(T entity)
		{
			context.Set<T>().Remove(entity);
			//return context.SaveChanges();
		}

		public IEnumerable<T> GetAll()
			=> context.Set<T>().ToList();

		public T GetById(int? id)
			=> context.Set<T>().Find(id);

		public void Update(T entity)
		{
			context.Set<T>().Update(entity);
			//return context.SaveChanges();
		}
	}
}
