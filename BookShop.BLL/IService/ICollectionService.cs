using BookShop.BLL.ConfigurationModel.BookGenreCategoryModel;
using BookShop.BLL.ConfigurationModel.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface ICollectionService
	{
		public Task<bool> add(CreateCollectionModel requet);
		public Task<bool> remove(int id);
		public Task<bool> update(int id, UpdateCollectionModel requet);
		public Task<List<CollectionModel>> Getall();
		public Task<CollectionModel> GetbyId(int id);
	}
}
