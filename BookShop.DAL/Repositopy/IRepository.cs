using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Repositopy
{
    public partial  interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(int id,T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> RemoveAsync(int id);
        Task<T> RemoveAsync(T obj);
    }
}
