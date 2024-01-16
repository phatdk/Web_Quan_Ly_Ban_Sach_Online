using BookShop.BLL.ConfigurationModel.CollectionBookModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin,Staff")]
	public class CollectionBook : Controller
    {
        public ICollectionService _CollectionSerVice;
        public IProductBookService _ProductBookService;
        public CollectionBook()
        {
            _CollectionSerVice = new CollectionService();
            _ProductBookService = new ProductBookService();
        }
        public async Task<IActionResult> Index()
        {
            var collectionModels = await _CollectionSerVice.GetAll();

            return View(collectionModels);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(",Name,Createdate,Status")] UpdateCollectionModel model)
        {
            if (id != _CollectionSerVice.GetAll().Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _CollectionSerVice.Update(id, model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_CollectionSerVice.GetById(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Createdate,Status")] CreateCollectionModel createCollection)
        {
            if (ModelState.IsValid)
            {

                _CollectionSerVice.Add(createCollection);

                return RedirectToAction(nameof(Index));
            }
            return View(createCollection);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var colection = _CollectionSerVice.GetById(id);
            if (colection.Id == null)
            {
                return NotFound();
            }

            return View(colection);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var collection = await _CollectionSerVice.GetById(id);
            _CollectionSerVice.Delete(collection.Id);
            return RedirectToAction(nameof(Index));
        }





    }
}
