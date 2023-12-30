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
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Image> _imageRepository;
        private readonly IRepository<ProductBook> _productBookRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IRepository<BookGenre> _bookGenreRepository;
        private readonly IRepository<CollectionBook> _collectionBookRepository;
        private readonly IRepository<Evaluate> _CommentRepository;
        public ProductService()
        {
            _productRepository = new Repository<Product>();
            _imageRepository = new Repository<Image>();
            _productBookRepository = new Repository<ProductBook>();
            _bookRepository = new Repository<Book>();
            _bookAuthorRepository = new Repository<BookAuthor>();
            _bookGenreRepository = new Repository<BookGenre>();
            _collectionBookRepository = new Repository<CollectionBook>();
            _CommentRepository = new Repository<Evaluate>();
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
                obj.ImgUrl = images.First().ImageUrl;
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
                    obj.CollectionName = productV.Id_Collection != null ? (await _collectionBookRepository.GetAllAsync()).Where(x => x.Id == productV.Id_Collection).FirstOrDefault().Name : "Trống";
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
                    obj.ImgUrl = images.First().ImageUrl;
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
                    obj.ImgUrl = images.First().ImageUrl;
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
                ImgUrl = imagevms.First().ImageUrl,
                CollectionId = product.Id_Collection,
                CollectionName = product.Id_Collection != null ? (await _collectionBookRepository.GetAllAsync()).Where(x => x.Id == product.Id_Collection).FirstOrDefault().Name : "Trống",
            };
            return obj;
        }

        public async Task<ProductViewModel> GetByIdAndCommnet(int id)
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

            var image = (await _imageRepository.GetAllAsync()).Where(x => x.Id_Product == id).OrderBy(x=>x.Index);
            var images = new List<ImageViewModel>();
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
                images.Add(imagev);
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
                imageViewModels = images,
                CollectionId = product.Id_Collection,
                CollectionName = product.Id_Collection != null ? (await _collectionBookRepository.GetAllAsync()).Where(x => x.Id == product.Id_Collection).FirstOrDefault().Name : "Trống",
                Comment = (await _CommentRepository.GetAllAsync()).Where(x => x.Id_Product == id).Select(x => new ConfigurationModel.EvaluateModel.EvaluateViewModel()
                {
                    Point = x.Point,
                    Content = x.Content,
                    CreatedDate = DateTime.Now,
                    Id_Product = x.Id_Product,
                    Id_User = x.Id_User,
                    Id_Parents = x.Id_Parents,
                    Id = x.Id
                }).ToList(),
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
    }
}
