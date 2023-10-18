using BookShop.BLL.ConfigurationModel.ProductBooklModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
	public class ProductBookService : IProductBookService
	{
		private readonly IRepository<ProductBook> _productDetailRepository;
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<Book> _bookRepository;
        public ProductBookService()
        {
            _productDetailRepository = new Repository<ProductBook>();
			_productRepository = new Repository<Product>();
			_bookRepository = new Repository<Book>();
        }
        public async Task<bool> Add(ProductBook model)
		{
			try
			{
				await _productDetailRepository.CreateAsync(model);
				return true;	
			}catch (Exception ex) { return  false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _productDetailRepository.RemoveAsync(id);
				return true;
			}catch (Exception ex) { return false; }
		}

		public async Task<List<ProductBookViewModel>> GetByBook(int bookId)
		{
			var details = (await _productDetailRepository.GetAllAsync()).Where(c=>c.Id_Book == bookId);
			var products = await _productRepository.GetAllAsync();
			var books = await _bookRepository.GetAllAsync();
			var objlist = (from a in details
						   join b in products on a.Id_Product equals b.Id
						   join c in books on a.Id_Book equals c.Id
						   select new ProductBookViewModel()
						   {
							   Id = a.Id,
							   Id_Book = c.Id,
							   Id_Product = b.Id,
							   Status = a.Status,
							   BookTitle = c.Title,
							   ProductName = b.Name,
						   }).ToList();
			return objlist;
		}

		public async Task<List<ProductBookViewModel>> GetByProduct(int productId)
		{
			var details = (await _productDetailRepository.GetAllAsync()).Where(c => c.Id_Product == productId);
			var products = await _productRepository.GetAllAsync();
			var books = await _bookRepository.GetAllAsync();
			var objlist = (from a in details
						   join b in products on a.Id_Product equals b.Id
						   join c in books on a.Id_Book equals c.Id
						   select new ProductBookViewModel()
						   {
							   Id = a.Id,
							   Id_Book = c.Id,
							   Id_Product = b.Id,
							   Status = a.Status,
							   BookTitle = c.Title,
							   ProductName = b.Name,
						   }).ToList();
			return objlist;
		}

		public async Task<bool> Update(int id, ProductBook model)
		{
			try
			{
				var obj = await _productDetailRepository.GetByIdAsync(id);
				obj.Id_Book = model.Id_Book;
				obj.Status = model.Status;
				await _productDetailRepository.UpdateAsync(id, obj);
				return true;
			}catch(Exception ex) { return false; }
		}
	}
}
