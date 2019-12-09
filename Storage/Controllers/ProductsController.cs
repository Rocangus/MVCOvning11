using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace Storage.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StorageContext _context;

        public ProductsController(StorageContext context)
        {
            _context = context;
        }

        // GET: Products
        public IActionResult Index(string name, string category)
        {
            var products = from p in _context.Product select p;

            List<string> categories = new List<string>();
            categories.Add("Category");

            foreach (var item in products)
            {
                if (!categories.Contains(item.Category))
                {
                    categories.Add(item.Category);
                }
            }

            if(!string.IsNullOrEmpty(category) && category.Equals("Category"))
            {
                category = "";
            }
            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(s => s.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(s => s.Category.Equals(category));
            }
            if (!(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(category)))
            {
                products = products.Where(s => s.Name.Contains(name) && s.Category.Equals(category));
            }

            return View((products.ToList(), categories));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,OrderDate,Category,Shelf,Count,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                if (!_context.Categories.ToList().Contains(new Category() { Name = product.Name })) 
                {
                    _context.Add(new Category() { Name = product.Name });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,OrderDate,Category,Shelf,Count,Description")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        // GET: Products/Values
        public IActionResult Values()
        {
            return View(GetProductViews());
        }

        public IEnumerable<ProductViewModel> GetProductViews()
        {
            var products = _context.Product.ToListAsync();
            var views = new List<ProductViewModel>();

            foreach (var product in products.Result.ToList())
            {
                var view = new ProductViewModel();
                PopulateViewModelData(views, product, view);
            }

            return views;
        }

        private static void PopulateViewModelData(List<ProductViewModel> views, Product product, ProductViewModel view)
        {
            view.Name = product.Name;
            view.Price = product.Price;
            view.Count = product.Count;
            view.InventoryValue = product.Price * product.Count;
            views.Add(view);
        }
    }
}
