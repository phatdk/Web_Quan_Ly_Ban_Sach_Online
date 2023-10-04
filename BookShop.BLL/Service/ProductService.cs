using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.ConfigurationModel.ImageModel;
using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Image> _imageRepository;
        private readonly IRepository<ProductBook> _productBookRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IRepository<BookGenre> _bookGenreRepository;
        public ProductService()
        {
            _productRepository = new Repository<Product>();
            _imageRepository = new Repository<Image>();
            _productBookRepository = new Repository<ProductBook>();
            _bookRepository = new Repository<Book>();
            _bookAuthorRepository = new Repository<BookAuthor>();
            _bookGenreRepository = new Repository<BookGenre>();
        }
        public async Task<bool> Add(CreateProductModel model)
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
                };
                var product = await _productRepository.CreateAsync(obj);
                if (product != null)
                {
                    foreach (BookViewModel item in model.bookViewModels)
                    {
                        ProductBook productBook = new ProductBook()
                        {
                            Id_Product = product.Id,
                            Id_Book = item.Id,
                        };
                        await _productBookRepository.CreateAsync(productBook);
                    }
                    foreach (ImageViewModel item in model.imageViewModels)
                    {
                        Image image = new Image()
                        {
                            ImageUrl = item.ImageUrl,
                            Index = item.Index,
                            CreatedDate = DateTime.Now,
                            Id_Product = product.Id,
                        };
                        await _imageRepository.CreateAsync(image);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex) { return false; }
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
                objlist.Add(obj);
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
                    products.Add(obj);
                }
            }
            return products;
        }

        public async Task<ProductViewModel> GetById(int id)
        {
            var obj = await _productRepository.GetByIdAsync(id);
            var book = await _bookRepository.GetByIdAsync((await _productBookRepository.GetAllAsync()).Where(x => x.Id_Product == obj.Id).Min(x=>x.Id));
            var books = new List<BookViewModel>()
            {
                new BookViewModel
                {
                    Id = book.Id,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Description = book.Description,
                    CreatedDate = book.CreatedDate,
                    Status = book.Status,
                    Reader = book.Reader,
                    Pages = book.Pages,
                    PageSize = book.PageSize,
                    Cover = book.Cover,
                    PublicationDate = book.PublicationDate,
                    Weight = book.Weight,
                    Widght = book.Widght,
                    Length = book.Length,
                    Height = book.Height,
                }
            };
            var image = (await _imageRepository.GetAllAsync()).Where(x => x.Id_Product == id);
            var imgages = new List<ImageViewModel>();
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
                imgages.Add(imagev);
            }
            return new ProductViewModel()
            {
                Id = obj.Id,
                Name = obj.Name,
                Price = obj.Price,
                Quantity = obj.Quantity,
                Description = obj.Description,
                CreatedDate = obj.CreatedDate,
                Status = obj.Status,
                Type = obj.Type,
                bookViewModels = books,
                imageViewModels = imgages
            };
        }

        public async Task<ProductViewModel> GetProductComboById(int id)
        {
            var obj = await _productRepository.GetByIdAsync(id);
            var pb = (await _productBookRepository.GetAllAsync()).Where(x => x.Id_Product == obj.Id);
            var books = new List<BookViewModel>();
            foreach (var item in pb)
            {
                var book = await _bookRepository.GetByIdAsync(item.Id_Book);
                var bookv = new BookViewModel()
                {
                    Id = book.Id,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Description = book.Description,
                    CreatedDate = book.CreatedDate,
                    Status = book.Status,
                    Reader = book.Reader,
                    Pages = book.Pages,
                    PageSize = book.PageSize,
                    Cover = book.Cover,
                    PublicationDate = book.PublicationDate,
                    Weight = book.Weight,
                    Widght = book.Widght,
                    Length = book.Length,
                    Height = book.Height,
                };
                books.Add(bookv);
            }
            
            var image = (await _imageRepository.GetAllAsync()).Where(x => x.Id_Product == id);
            var imgages = new List<ImageViewModel>();
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
                imgages.Add(imagev);
            }
            return new ProductViewModel()
            {
                Id = obj.Id,
                Name = obj.Name,
                Price = obj.Price,
                Quantity = obj.Quantity,
                Description = obj.Description,
                CreatedDate = obj.CreatedDate,
                Status = obj.Status,
                Type = obj.Type,
                bookViewModels = books,
                imageViewModels = imgages
            };
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
                await _productRepository.UpdateAsync(obj.Id, obj);
                foreach(var item in model.imageViewModels)
                {
                    var img = await _imageRepository.GetByIdAsync(item.Id);
                    if(img.ImageUrl != item.ImageUrl)
                    {
                        img.ImageUrl = item.ImageUrl;
                        await _imageRepository.UpdateAsync(img.Id, img);
                    }
                }
                //foreach(var item in model.bookViewModels)
                //{
                //    var listpb = (await _productBookRepository.GetAllAsync()).Where(x=>x.Id_Product == obj.Id);
                //    foreach(var pb in listpb)
                //    {
                //        if()
                //    }
                //}
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}
