using BookShop.DAL.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Repositopy
{
    public partial class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbcontext _applicationDbcontext;
        public Repository() 
        {
            _applicationDbcontext = new ApplicationDbcontext();
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            _applicationDbcontext.Set<TEntity>().Add(entity);
            await _applicationDbcontext.SaveChangesAsync();
            return entity;
        }


        public async Task<List<TEntity>> GetAllAsync()
        {
         var obj = await  _applicationDbcontext.Set<TEntity>().ToListAsync();
            return obj;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var obj = await _applicationDbcontext.Set<TEntity>().FindAsync(id);
            return obj;
        }

        public async Task<TEntity> UpdateAsync(int id, TEntity updatedEntity)
        {
            var existingEntity = await _applicationDbcontext.Set<TEntity>().FindAsync(id);

            if (existingEntity != null)
            {
                _applicationDbcontext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
                await _applicationDbcontext.SaveChangesAsync();
            }

            return existingEntity;
        }

    }
}
