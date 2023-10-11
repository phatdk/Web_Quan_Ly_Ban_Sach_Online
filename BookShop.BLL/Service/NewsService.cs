using BookShop.BLL.ConfigurationModel.NewsModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
	public class NewsService : INewsService
	{
		private readonly IRepository<News> _repository;
		public NewsService()
		{
			_repository = new Repository<News>();
		}

		public async Task<bool> Add(CreateNewsModel model)
		{
			try
			{
				var obj = new News()
				{
					Title = model.Title,
					Content = model.Content,
					Description = model.Description,
					CreatedDate = DateTime.Now,
					Status = model.Status,
				};
				await _repository.CreateAsync(obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _repository.RemoveAsync(id);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<List<NewsViewModel>> GetAll()
		{
			var list = await _repository.GetAllAsync();
			var objlist = new List<NewsViewModel>();
			foreach (var item in list)
			{
				var obj = new NewsViewModel()
				{
					Id = item.Id,
					Title = item.Title,
					Content = item.Content,
					Description = item.Description,
					CreatedDate = item.CreatedDate,
					Status = item.Status,
				};
				objlist.Add(obj);
			}
			return objlist;
		}

		public async Task<NewsViewModel> GetById(int id)
		{
			var obj = await _repository.GetByIdAsync(id);
			return new NewsViewModel()
			{
				Id = obj.Id,
				Title = obj.Title,
				Content = obj.Content,
				Description = obj.Description,
				CreatedDate = obj.CreatedDate,
				Status = obj.Status,
			};
		}

		public async Task<List<NewsViewModel>> GetByStatus(int status)
		{
			var list = (await _repository.GetAllAsync()).Where(c => c.Status == status);
			var objlist = new List<NewsViewModel>();
			foreach (var item in list)
			{
				var obj = new NewsViewModel()
				{
					Id = item.Id,
					Title = item.Title,
					Content = item.Content,
					Description = item.Description,
					CreatedDate = item.CreatedDate,
					Status = item.Status,
				};
				objlist.Add(obj);
			}
			return objlist;
		}

		public async Task<bool> Update(int id, UpdateNewsModel model)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				obj.Title = model.Title;
				obj.Content = model.Content;
				obj.Description = model.Description;
				obj.Status = model.Status;
				await _repository.UpdateAsync(id, obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
