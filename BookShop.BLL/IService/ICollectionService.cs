using BookShop.BLL.ConfigurationModel.GenreModel;
using BookShop.BLL.ConfigurationModel.CollectionBookModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface ICollectionService
	{
		public Task<bool> Add(CreateCollectionModel requet);
		public Task<bool> Delete(int id);
		public Task<bool> Update(int id, UpdateCollectionModel requet);
		public Task<List<CollectionModel>> Getall();
		public Task<CollectionModel> GetById(int id);
	}
}
