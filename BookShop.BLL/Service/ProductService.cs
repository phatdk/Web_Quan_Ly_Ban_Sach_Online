using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.ConfigurationModel.ImageModel;
using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.Gembox;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;
using Image = BookShop.DAL.Entities.Image;


namespace BookShop.BLL.Service
{

	public class ProductService : IProductService
	{
		private readonly IRepository<Product> _productRepository;//12
		private readonly IRepository<Image> _imageRepository;//11
		private readonly IRepository<ProductBook> _productBookRepository;//10
		private readonly IRepository<Book> _bookRepository;//9
		private readonly IRepository<BookAuthor> _bookAuthorRepository;//8
		private readonly IRepository<BookGenre> _bookGenreRepository;//7
		private readonly IRepository<CollectionBook> _collectionBookRepository;//6
		private readonly IRepository<Evaluate> _CommentRepository;
		private readonly IRepository<Genre> _genretRepository;//5
		private readonly IRepository<Category> _categorytRepository;//4
		private readonly IRepository<ProductPromotion> _productPromotionRepository;//3
		private readonly IRepository<Category> _categoriesRepository;//2
		private readonly IRepository<Promotion> _promotionRepository; //1
		private readonly IRepository<PromotionType> _promotionTypeRepository; //0
		private readonly IRepository<Author> _authorRepository; //0
		private readonly IRepository<Supplier> _supplierRepository; //13
		public ProductService()
		{
			_supplierRepository = new Repository<Supplier>();
			_productRepository = new Repository<Product>();
			_imageRepository = new Repository<Image>();
			_productBookRepository = new Repository<ProductBook>();
			_bookRepository = new Repository<Book>();
			_bookAuthorRepository = new Repository<BookAuthor>();
			_bookGenreRepository = new Repository<BookGenre>();
			_collectionBookRepository = new Repository<CollectionBook>();
			_CommentRepository = new Repository<Evaluate>();
			_genretRepository = new Repository<Genre>();
			_categorytRepository = new Repository<Category>();
			_promotionTypeRepository = new Repository<PromotionType>();
			_promotionRepository = new Repository<Promotion>();
			_productPromotionRepository = new Repository<ProductPromotion>();
			_authorRepository = new Repository<Author>();
            _supplierRepository = new Repository<Supplier>();

		}
		public async Task<CreateProductModel> Add(CreateProductModel model)
		{
			try
			{
				var obj = new Product()
				{
					Name = model.Name,
					Price = model.Price,
					Quantity = model.Quantity,
					Description = model.Description,
					CreatedDate = DateTime.Now,
					Status = model.Status,
					Type = model.Type,
					Id_Collection = model.CollectionId,
				};
				var product = await _productRepository.CreateAsync(obj);
				if (product != null)
				{
					foreach (var item in model.bookSelected)
					{
						ProductBook productBook = new ProductBook()
						{
							Id_Product = product.Id,
							Id_Book = item,
							Status = product.Status,
						};
						await _productBookRepository.CreateAsync(productBook);
					}
					return new CreateProductModel()
					{
						Id = product.Id,
						Name = model.Name,
					};
				}
				return model;
			}
			catch (Exception ex) { return model; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _productRepository.RemoveAsync(id);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<List<ProductViewModel>> GetAll()
		{
			var list = await _productRepository.GetAllAsync();
			var objlist = new List<ProductViewModel>();
			foreach (var item in list)
			{
				var obj = new ProductViewModel()
				{
					Id = item.Id,
					Name = item.Name,
					Price = item.Price,
					Quantity = item.Quantity,
					Description = item.Description,
					CreatedDate = item.CreatedDate,
					Status = item.Status,
					Type = item.Type,
				};
				obj.CollectionName = item.Id_Collection != null ? (await _collectionBookRepository.GetAllAsync()).Where(x => x.Id == item.Id_Collection).FirstOrDefault().Name : "Trống";
				var images = (await _imageRepository.GetAllAsync()).Where(x => x.Id_Product == item.Id).OrderBy(x => x.Index);
				var imagevms = new List<ImageViewModel>();
				foreach (var image in images)
				{
					var imagevm = new ImageViewModel()
					{
						Id = image.Id,
						ImageUrl = image.ImageUrl,
						Index = image.Index,
						CreatedDate = image.CreatedDate,
						Status = image.Status,
						Id_Product = image.Id_Product,
					};
					imagevms.Add(imagevm);
				}
				obj.imageViewModels = imagevms;
				obj.ImgUrl = images != null ? images.FirstOrDefault().ImageUrl : "image null";
				objlist.Add(obj);
			}
			//return objlist;
			//var query = (from
			//				 p in (await _productRepository.GetAllAsync()).DefaultIfEmpty()
			//			 join i in (await _imageRepository.GetAllAsync()).DefaultIfEmpty() on p.Id equals i.Id_Product into imgGroup
			//			 join pb in (await _productBookRepository.GetAllAsync()).DefaultIfEmpty() on p.Id equals pb.Id_Product
			//			 join b in (await _bookRepository.GetAllAsync()).DefaultIfEmpty() on pb.Id_Book equals b.Id
			//			 join ba in (await _bookAuthorRepository.GetAllAsync()).DefaultIfEmpty() on b.Id equals ba.Id_Book
			//			 join a in (await _authorRepository.GetAllAsync()).DefaultIfEmpty() on ba.Id_Author equals a.Id
			//			 join bg in (await _bookGenreRepository.GetAllAsync()).DefaultIfEmpty() on b.Id equals bg.Id_Book
			//			 join g in (await _genretRepository.GetAllAsync()).DefaultIfEmpty() on bg.Id_Genre equals g.Id
			//			 join c in (await _categorytRepository.GetAllAsync()).DefaultIfEmpty() on g.Id_Category equals c.Id

			//			 //where g.Id == gennerId || c.Id == categoriId || a.Id == authorId || p.Price > min
			//			 select new ProductViewModel()
			//			 {
			//				 Id = p.Id,
			//				 Name = p.Name,
			//				 Price = p.Price,
			//				 Type = p.Type,
			//				 Quantity = p.Quantity,
			//				 Status = p.Status,
			//				 ImgUrl = imgGroup.FirstOrDefault()?.ImageUrl,
			//			 }).Distinct().ToList();

			var pp = (await _productPromotionRepository.GetAllAsync()).Where(c => c.Status == 1);
			var pr = (await _promotionRepository.GetAllAsync());
			var prt = (await _promotionTypeRepository.GetAllAsync());
			var cb = (await _collectionBookRepository.GetAllAsync());

			foreach (var item in objlist)
			{
				item.NewPrice = item.Price;
				item.Saleoff = 0;
				var km = pp.Where(c => c.Id_Product == item.Id).OrderBy(c => c.CreatedDate).FirstOrDefault();

				if (km != null)
				{
					var tkm = pr.FirstOrDefault(c => c.Id == km.Id_Promotion);
					if (tkm != null)
					{
						DateTime startTime = Convert.ToDateTime(tkm.StartDate);
						DateTime endTime = Convert.ToDateTime(tkm.EndDate);
						int result = DateTime.Now.CompareTo(startTime);
						if (result >= 0 && endTime.CompareTo(DateTime.Now) >= 0)
						{
							item.Saleoff = tkm.PercentReduct;
							item.NewPrice = Convert.ToInt32((item.Price * 100) - (item.Price * tkm.PercentReduct)) / 100;
						}

					}
				}
			}
			return objlist;
		}


		public async Task<List<ProductViewModel>> GetByAuthor(int authorId)
		{
			var listAuthor = (await _bookAuthorRepository.GetAllAsync()).Where(x => x.Id_Author == authorId);
			var products = new List<ProductViewModel>();
			foreach (BookAuthor item in listAuthor)
			{
				var productB = (await _productBookRepository.GetAllAsync()).Where(x => x.Id_Book == item.Id_Book);
				foreach (ProductBook item1 in productB)
				{
					var productV = await _productRepository.GetByIdAsync(item1.Id_Product);
					ProductViewModel obj = new ProductViewModel()
					{
						Id = productV.Id,
						Name = productV.Name,
						Price = productV.Price,
						Quantity = productV.Quantity,
						Description = productV.Description,
						CreatedDate = productV.CreatedDate,
						Status = productV.Status,
						Type = productV.Type,
					};
					obj.CollectionName = productV.Id_Collection != null ? (await _collectionBookRepository.GetAllAsync()).Where(x => x.Id == productV.Id_Collection).FirstOrDefault().Name : "Trống";
					var images = (await _imageRepository.GetAllAsync()).Where(x => x.Id_Product == productV.Id).OrderBy(x => x.Index);
					var imagevms = new List<ImageViewModel>();
					foreach (var image in images)
					{
						var imagevm = new ImageViewModel()
						{
							Id = image.Id,
							ImageUrl = image.ImageUrl,
							Index = image.Index,
							CreatedDate = image.CreatedDate,
							Status = image.Status,
							Id_Product = image.Id_Product,
						};
						imagevms.Add(imagevm);
					}
					obj.imageViewModels = imagevms;
					obj.ImgUrl = images != null ? images.FirstOrDefault().ImageUrl : "image null";
					products.Add(obj);
				}
			}
			return products;
		}

		public async Task<List<ProductViewModel>> GetByGenre(int genreId)
		{
			var listGenre = (await _bookGenreRepository.GetAllAsync()).Where(x => x.Id_Genre == genreId);
			var products = new List<ProductViewModel>();
			foreach (BookGenre item in listGenre)
			{
				var productB = (await _productBookRepository.GetAllAsync()).Where(x => x.Id_Book == item.Id_Book);
				foreach (ProductBook item1 in productB)
				{
					var productV = await _productRepository.GetByIdAsync(item1.Id_Product);
					ProductViewModel obj = new ProductViewModel()
					{
						Id = productV.Id,
						Name = productV.Name,
						Price = productV.Price,
						Quantity = productV.Quantity,
						Description = productV.Description,
						CreatedDate = productV.CreatedDate,
						Status = productV.Status,
						Type = productV.Type,
					};
					obj.CollectionName = productV.Id_Collection != null ? (await _collectionBookRepository.GetAllAsync()).Where(x => x.Id == productV.Id_Collection).FirstOrDefault().Name : "Trống";
					var images = (await _imageRepository.GetAllAsync()).Where(x => x.Id_Product == productV.Id).OrderBy(x => x.Index);
					var imagevms = new List<ImageViewModel>();
					foreach (var image in images)
					{
						var imagevm = new ImageViewModel()
						{
							Id = image.Id,
							ImageUrl = image.ImageUrl,
							Index = image.Index,
							CreatedDate = image.CreatedDate,
							Status = image.Status,
							Id_Product = image.Id_Product,
						};
						imagevms.Add(imagevm);
					}
					obj.imageViewModels = imagevms;
					obj.ImgUrl = images != null ? images.FirstOrDefault().ImageUrl : "image null";
					products.Add(obj);
				}
			}
			return products;
		}

		public async Task<ProductViewModel> GetById(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			var pb = (await _productBookRepository.GetAllAsync()).Where(x => x.Id_Product == id);
			var books = new List<BookViewModel>();
			foreach (var item in pb)
			{
				var book = await _bookRepository.GetByIdAsync(item.Id_Book);
				var bookvm = new BookViewModel()
				{
					Id = book.Id,
					Title = book.Title,
					ImportPrice = book.ImportPrice,
					Status = book.Status,
					Weight = book.Weight,
					Widght = book.Widght,
					Length = book.Length,
					Height = book.Height,
				};
				books.Add(bookvm);
			}

			var image = (await _imageRepository.GetAllAsync()).Where(x => x.Id_Product == id).OrderBy(x => x.Index);
			var imagevms = new List<ImageViewModel>();
			foreach (var item in image)
			{
				var imagev = new ImageViewModel()
				{
					Id = item.Id,
					ImageUrl = item.ImageUrl,
					Index = item.Index,
					CreatedDate = item.CreatedDate,
					Status = item.Status,
				};
				imagevms.Add(imagev);
			}

			var obj = new ProductViewModel()
			{
				Id = product.Id,
				Name = product.Name,
				Price = product.Price,
				Quantity = product.Quantity,
				Description = product.Description,
				CreatedDate = product.CreatedDate,
				Status = product.Status,
				Type = product.Type,
				bookViewModels = books,
				imageViewModels = imagevms,
				ImgUrl = imagevms != null ? imagevms.FirstOrDefault().ImageUrl : "image null",
				CollectionId = product.Id_Collection,
				CollectionName = product.Id_Collection != null ? (await _collectionBookRepository.GetAllAsync()).Where(x => x.Id == product.Id_Collection).FirstOrDefault().Name : "Trống",
			};
			obj.NewPrice = obj.Price;
			obj.Saleoff = 0;
			var pp = (await _productPromotionRepository.GetAllAsync()).Where(x => x.Status == 1 && x.Id_Product == id).OrderBy(x => x.CreatedDate);
			foreach (var item in pp)
			{
				var promotion = await _promotionRepository.GetByIdAsync(item.Id_Promotion);
				if (promotion != null)
				{
					DateTime startTime = Convert.ToDateTime(promotion.StartDate);
					DateTime endTime = Convert.ToDateTime(promotion.EndDate);
					int result = DateTime.Now.CompareTo(startTime);
					if (result >= 0 && endTime.CompareTo(DateTime.Now) >= 0)
					{
						obj.Saleoff = promotion.PercentReduct;
						obj.NewPrice = Convert.ToInt32((obj.Price * 100) - (obj.Price * promotion.PercentReduct)) / 100;
					}
				}
			}
			return obj;
		}

		public async Task<ProductViewModel> GetByIdAndCommnet(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			var obj = new ProductViewModel()
			{
				Id = product.Id,
				Name = product.Name,
				Price = product.Price,
				Quantity = product.Quantity,
				Description = product.Description,
				CreatedDate = product.CreatedDate,
				Status = product.Status,
				Type = product.Type,
				CollectionId = product.Id_Collection,
				CollectionName = product.Id_Collection != null ? (await _collectionBookRepository.GetAllAsync()).Where(x => x.Id == product.Id_Collection).FirstOrDefault().Name : "",
				//Comment = (await _CommentRepository.GetAllAsync()).Where(x => x.Id_Product == id).Select(x => new ConfigurationModel.EvaluateModel.EvaluateViewModel()
				//{
				//	Point = x.Point,
				//	Content = x.Content,
				//	CreatedDate = DateTime.Now,
				//	Id_Product = x.Id_Product,
				//	Id_User = x.Id_User,
				//	Id_Parents = x.Id_Parents,
				//	Id = x.Id
				//}).ToList(),
				bookViewModels = new List<BookViewModel>(),
				imageViewModels = new List<ImageViewModel>(),
				authorModels = new List<ConfigurationModel.AuthorModel.AuthorModel>(),
				supplierModels = new List<ConfigurationModel.SupplierModel.SupplierViewModel>(),
				CoverBook = new List<string>(),
			};

			var pb = (await _productBookRepository.GetAllAsync()).Where(x => x.Id_Product == id);
			var ba = await _bookAuthorRepository.GetAllAsync();
			foreach (var item in pb)
			{
				var book = await _bookRepository.GetByIdAsync(item.Id_Book);
				var bookvm = new BookViewModel()
				{
					Id = book.Id,
					Title = book.Title,
					ImportPrice = book.ImportPrice,
					Status = book.Status,
					Weight = book.Weight,
					Widght = book.Widght,
					Length = book.Length,
					Height = book.Height,
				};
				obj.bookViewModels.Add(bookvm);
				var bookau = ba.Where(x => x.Id_Book == item.Id_Book);
				foreach (var authorId in bookau) // tt tac gia
				{
					if ((obj.authorModels.Where(x => x.Id == authorId.Id_Author)) == null)
					{
						var author = await _authorRepository.GetByIdAsync(authorId.Id_Author);
						if (author != null)
						{
							obj.authorModels.Add(new ConfigurationModel.AuthorModel.AuthorModel
							{
								Id = author.Id,
								Name = author.Name,
							});
						}
					}
				}
				var suplier = await _supplierRepository.GetByIdAsync(book.Id_Supplier);
				if (obj.supplierModels.Where(x => x.Id == suplier.Id) == null)
				{
					obj.supplierModels.Add(new ConfigurationModel.SupplierModel.SupplierViewModel
					{
						Id = suplier.Id,
						Name = suplier.Name,
					});
				}
				foreach (var cover in obj.CoverBook)
				{
					if (!book.Cover.Equals(cover))
					{
						obj.CoverBook.Add(book.Cover);
					}
				}
			}

			var image = (await _imageRepository.GetAllAsync()).Where(x => x.Id_Product == id).OrderBy(x => x.Index);
			foreach (var item in image)
			{
				var imagev = new ImageViewModel()
				{
					Id = item.Id,
					ImageUrl = item.ImageUrl,
					Index = item.Index,
					CreatedDate = item.CreatedDate,
					Status = item.Status,
				};
				obj.imageViewModels.Add(imagev);
			}
			obj.ImgUrl = obj.imageViewModels.First().ImageUrl;

			obj.NewPrice = obj.Price;
			obj.Saleoff = 0;
			var pp = (await _productPromotionRepository.GetAllAsync()).Where(x => x.Status == 1 && x.Id_Product == id).OrderBy(x => x.CreatedDate);
			foreach (var item in pp)
			{
				var promotion = await _promotionRepository.GetByIdAsync(item.Id_Promotion);
				if (promotion != null)
				{
					DateTime startTime = Convert.ToDateTime(promotion.StartDate);
					DateTime endTime = Convert.ToDateTime(promotion.EndDate);
					int result = DateTime.Now.CompareTo(startTime);
					if (result >= 0 && endTime.CompareTo(DateTime.Now) >= 0)
					{
						obj.Saleoff = promotion.PercentReduct;
						obj.NewPrice = Convert.ToInt32((obj.Price * 100) - (obj.Price * promotion.PercentReduct)) / 100;
					}
				}
			}
			return obj;
		}

		public async Task<bool> Update(UpdateProductModel model)
		{
			try
			{
				var obj = await _productRepository.GetByIdAsync(model.Id);
				obj.Name = model.Name;
				obj.Price = model.Price;
				obj.Quantity = model.Quantity;
				obj.Description = model.Description;
				obj.Status = model.Status;
				obj.Type = model.Type;
				obj.Id_Collection = model.CollectionId;
				await _productRepository.UpdateAsync(obj.Id, obj);

				var pbs = (await _productBookRepository.GetAllAsync()).Where(x => x.Id_Product == obj.Id);
				// Loại bỏ phần tử không có và skip các phần tử đã có
				foreach (var item in pbs)
				{
					for (int i = 0; i < model.bookSelected.Count(); i++)
					{
						if (model.bookSelected[i] == item.Id_Book)
						{
							model.bookSelected.RemoveAt(i);
							goto skip;
						}
					}
					await _productBookRepository.RemoveAsync(item.Id);
				skip:;
				}
				// tạo phần tử mới
				foreach (var item in model.bookSelected)
				{
					var pbnew = new ProductBook()
					{
						Id_Product = model.Id,
						Id_Book = item,
					};
					await _productBookRepository.CreateAsync(pbnew);
				}
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<bool> ChangeQuantity(int id, int changeAmount)
		{
		getAgain:;
			var product = await _productRepository.GetByIdAsync(id);
			try
			{
				if (product != null)
				{
					product.Quantity += changeAmount;
					if (product.Quantity <= 0)
					{
						product.Status = 2;
					}
				}
				else goto getAgain;
				await _productRepository.UpdateAsync(id, product);
				return true;
			}
			catch { return false; }
		}

		public async Task<List<ProductViewModel>> GetDanhMuc(string danhmuc)
		{

			var productsInCategory = (
				from category in (await _categorytRepository.GetAllAsync()).DefaultIfEmpty()
				join genre in (await _genretRepository.GetAllAsync()).DefaultIfEmpty() on category.Id equals genre.Id_Category
				join bookGenre in (await _bookGenreRepository.GetAllAsync()).DefaultIfEmpty() on genre.Id equals bookGenre.Id_Genre
				join book in (await _bookRepository.GetAllAsync()).DefaultIfEmpty() on bookGenre.Id_Book equals book.Id
				join productBook in (await _productBookRepository.GetAllAsync()).DefaultIfEmpty() on book.Id equals productBook.Id_Book
				join product in (await _productRepository.GetAllAsync()).DefaultIfEmpty() on productBook.Id_Product equals product.Id
				join image in (await _imageRepository.GetAllAsync()).DefaultIfEmpty() on product.Id equals image.Id_Product
				where category.Name == danhmuc
				select new ProductViewModel
				{
					Id = product.Id,
					Name = product.Name,
					Price = product.Price,
					Quantity = product.Quantity,
					Description = product.Description,
					CreatedDate = product.CreatedDate,
					Status = product.Status,
					ImgUrl = image.ImageUrl,

				}
			).Distinct().ToList();
			var pp = (await _productPromotionRepository.GetAllAsync()).Where(c => c.Status == 1);
			var pr = (await _promotionRepository.GetAllAsync());
			var prt = (await _promotionTypeRepository.GetAllAsync());
			var cb = (await _collectionBookRepository.GetAllAsync());

			foreach (var item in productsInCategory)
			{
				item.NewPrice = item.Price;
				item.Saleoff = 0;
				var km = pp.Where(c => c.Id_Product == item.Id).OrderBy(c => c.CreatedDate).FirstOrDefault();

				if (km != null)
				{
					var tkm = pr.FirstOrDefault(c => c.Id == km.Id_Promotion);
					if (tkm != null)
					{
						item.Saleoff = tkm.PercentReduct;
						item.NewPrice = Convert.ToInt32((item.Price * 100) - (item.Price * tkm.PercentReduct)) / 100;

					}
				}
			}
			return productsInCategory;

		}

		public async Task<List<ProductViewModel>> Search(int? gennerId, int? supplierId, int? authorId)
		{


			var query = (from
						 p in (await _productRepository.GetAllAsync()).DefaultIfEmpty()
						 join i in (await _imageRepository.GetAllAsync()).DefaultIfEmpty() on p.Id equals i.Id_Product into imgGroup
						 join pb in (await _productBookRepository.GetAllAsync()).DefaultIfEmpty() on p.Id equals pb.Id_Product
						 join b in (await _bookRepository.GetAllAsync()).DefaultIfEmpty() on pb.Id_Book equals b.Id
						 join ba in (await _bookAuthorRepository.GetAllAsync()).DefaultIfEmpty() on b.Id equals ba.Id_Book
						 join a in (await _authorRepository.GetAllAsync()).DefaultIfEmpty() on ba.Id_Author equals a.Id
						 join bg in (await _bookGenreRepository.GetAllAsync()).DefaultIfEmpty() on b.Id equals bg.Id_Book
						 join g in (await _genretRepository.GetAllAsync()).DefaultIfEmpty() on bg.Id_Genre equals g.Id
						 join sup in (await _supplierRepository.GetAllAsync()).DefaultIfEmpty() on b.Id_Supplier equals sup.Id

						 where g.Id == gennerId || sup.Id == supplierId || a.Id == authorId 
						 select new ProductViewModel()
						 {
							 Id = p.Id,
							 Name = p.Name,
							 Price = p.Price,
							 Quantity = p.Quantity,

							 Status = p.Status,
							 ImgUrl = imgGroup.FirstOrDefault()?.ImageUrl,
						 }).Distinct().ToList();

			var pp = (await _productPromotionRepository.GetAllAsync()).Where(c => c.Status == 1);
			var pr = (await _promotionRepository.GetAllAsync());
			var prt = (await _promotionTypeRepository.GetAllAsync());
			var cb = (await _collectionBookRepository.GetAllAsync());

			foreach (var item in query)
			{
				item.NewPrice = item.Price;
				item.Saleoff = 0;
				var km = pp.Where(c => c.Id_Product == item.Id).OrderBy(c => c.CreatedDate).FirstOrDefault();

				if (km != null)
				{
					var tkm = pr.FirstOrDefault(c => c.Id == km.Id_Promotion);
					if (tkm != null)
					{
						DateTime startTime = Convert.ToDateTime(tkm.StartDate);
						DateTime endTime = Convert.ToDateTime(tkm.EndDate);
						int result = DateTime.Now.CompareTo(startTime);
						if (result >= 0 && endTime.CompareTo(DateTime.Now) >= 0)
						{
							item.Saleoff = tkm.PercentReduct;
							item.NewPrice = Convert.ToInt32((item.Price * 100) - (item.Price * tkm.PercentReduct)) / 100;
						}

					}
				}
			}
			return query;
		}


	}
}
