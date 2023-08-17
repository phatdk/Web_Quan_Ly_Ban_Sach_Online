using BLL.ConfigurationModel.AuthorModel;
using BLL.ConfigurationModel.UserModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IService.IAuthorService
{
	public interface IAuthorService
	{
		public Task<List<Author>> Getall();
		public Task<Author> GetbyId(int id);
		public Task<bool> Update(int id,UpdateAuthorModel requet);
		public Task<bool> Delete(int id);
		public Task<bool> Add(CreateAuthorModel requet);
	}
}
