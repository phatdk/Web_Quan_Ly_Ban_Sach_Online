using BookShop.BLL.ConfigurationModel.NewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface INewsService
	{
		public Task<List<NewsViewModel>> GetAll();
		public Task<List<NewsViewModel>> GetByStatus(int status);
		public Task<NewsViewModel> GetById(int id);
		public Task<bool> Add(CreateNewsModel model);
		public Task<bool> Update(int id, UpdateNewsModel model);
		public Task<bool> Delete(int id);
	}
}
