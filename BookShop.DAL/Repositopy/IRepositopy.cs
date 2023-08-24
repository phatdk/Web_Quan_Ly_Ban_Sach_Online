using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Repositopy
{
	public partial interface IRepository<TEntity> where TEntity : class
	{
		Task<TEntity> CreateAsync(TEntity entity);
		Task<TEntity> UpdateAsync(int id, TEntity entity);
		Task<List<TEntity>> GetAllAsync();
		Task<TEntity> GetByIdAsync(int id);
		Task<TEntity> RemoveAsync(int id);
	}
}
