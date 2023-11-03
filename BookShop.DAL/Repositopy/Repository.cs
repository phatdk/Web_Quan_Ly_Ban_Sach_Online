using BookShop.DAL.ApplicationDbContext;
using BookShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Repositopy
{
    public partial  class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbcontext _context;

		public Repository()
		{
			_context = new ApplicationDbcontext();
		}

		public async Task<T> CreateAsync(T entity)
        {
            var obj = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return obj.Entity;
        }


        public async Task<List<T>> GetAllAsync()
        {
            var obj = await  _context.Set<T>().ToListAsync();
            return obj;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var obj = await _context.Set<T>().FindAsync(id);
            return obj;
        }

		public async Task<T> RemoveAsync(int id)
		{
            var obj = await _context.Set<T>().FindAsync(id);
            if (obj != null)
            {
                _context.Set<T>().Remove(obj);
                await _context.SaveChangesAsync();
            }
            return obj;
        }
		public async Task<T> RemoveAsync(T obj)
		{
            //var obj = await _context.Set<T>().FindAsync(id);
            if (obj != null)
            {
                _context.Set<T>().Remove(obj);
                await _context.SaveChangesAsync();
            }
            return obj;
        }

		public async Task<T> UpdateAsync(int id, T updatedEntity)
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
                await _context.SaveChangesAsync();
            }

            return existingEntity;
        }

    }
}
