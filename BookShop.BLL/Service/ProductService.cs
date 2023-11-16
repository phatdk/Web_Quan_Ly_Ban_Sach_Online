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
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;
using Image = BookShop.DAL.Entities.Image;

namespace BookShop.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Image> _imageRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductBook> _productBookRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IRepository<BookGenre> _bookGenreRepository;
        private readonly IRepository<CollectionBook> _collectionBookRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        public ProductService()
        {
            _genreRepository = new Repository<Genre>();
            _authorRepository = new Repository<Author>();
            _categoryRepository = new Repository<Category>();
            _supplierRepository = new Repository<Supplier>();
            _productRepository = new Repository<Product>();
            _imageRepository = new Repository<Image>();
            _productBookRepository = new Repository<ProductBook>();
            _bookRepository = new Repository<Book>();
            _bookAuthorRepository = new Repository<BookAuthor>();
            _bookGenreRepository = new Repository<BookGenre>();
            _collectionBookRepository = new Repository<CollectionBook>();
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
                var img = (await _imageRepository.GetAllAsync()).Where(x => x.Id_Product == item.Id).FirstOrDefault();
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
                    ImgUrl = img.ImageUrl,
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

            string collecionName = string.Empty;
            if (product.Id_Collection != null)
            {
                collecionName = (await _collectionBookRepository.GetAllAsync()).Where(x => x.Id == product.Id_Collection).FirstOrDefault().Name;
            }
            return new ProductViewModel()
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
                imageViewModels = imgages,
                CollectionId = product.Id_Collection,
                CollectionName = collecionName,
            };
        }

        public async Task<List<ProductViewModel>> GetByCollection(int collectionId)
        {
            var products = (await _productRepository.GetAllAsync()).Where(x => x.Id_Collection == collectionId);
            var productsvm = new List<ProductViewModel>();
            foreach (var item in products)
            {
                var productvm = new ProductViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Status = item.Status,
                    Type = item.Type,
                };
                productsvm.Add(productvm);
            }
            return productsvm;
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
                }
                else goto getAgain;
                await _productRepository.UpdateAsync(id, product);
                return true;
            }
            catch { return false; }
        }

       

        public async Task<List<ProductViewModel>> filter(int ct, int gr, int sls, int cbk, int au)
        {
            var product = await _productRepository.GetAllAsync();
            var productBook = await _productBookRepository.GetAllAsync();
            var book = await _bookRepository.GetAllAsync();
            var bookAuthor = await _bookAuthorRepository.GetAllAsync();
            var bookGenre = await _bookGenreRepository.GetAllAsync();
            var collectionBook = await _collectionBookRepository.GetAllAsync();
            var genre = await _genreRepository.GetAllAsync();
            var author = await _authorRepository.GetAllAsync();
            var category = await _categoryRepository.GetAllAsync();
            var supplier = await _supplierRepository.GetAllAsync();
            var imager = await _imageRepository.GetAllAsync();
            var query =
                 (
                 from i in imager.DefaultIfEmpty() 
                 join p in product.DefaultIfEmpty() on i.Id_Product equals p.Id
                 join pb in productBook.DefaultIfEmpty() on p.Id equals pb.Id_Product
                 join b in book.DefaultIfEmpty() on pb.Id_Book equals b.Id
                 join ba in bookAuthor.DefaultIfEmpty() on b.Id equals ba.Id_Author
                 join bg in bookGenre.DefaultIfEmpty() on b.Id equals bg.Id_Book
                 join cb in collectionBook.DefaultIfEmpty() on p.Id_Collection equals cb.Id
                 join g in genre.DefaultIfEmpty() on bg.Id_Genre equals g.Id
                 join a in author.DefaultIfEmpty() on ba.Id_Author equals a.Id
                 join c in category.DefaultIfEmpty() on g.Id_Category equals c.Id
                 join sl in supplier.DefaultIfEmpty() on b.Id_Supplier equals sl.Id
                 where c.Id == ct || g.Id == gr || sl.Id == sls || cb.Id == cbk || a.Id == au
                 select new ProductViewModel
                 {
                     Id = p.Id,
                     Name = p.Name,
                     Price = p.Price,
                     Quantity = p.Quantity,
                     Description = p.Description,
                     CreatedDate = p.CreatedDate,
                     Status = p.Status,
                     Type = p.Type,
                     ImgUrl = i.ImageUrl,
                   
                 });

            var results = query.ToList();
            return results;
        }
    }
}
