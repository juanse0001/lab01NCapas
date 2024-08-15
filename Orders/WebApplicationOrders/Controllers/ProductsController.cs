using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using ProxyService.Interfaces;
using System.Threading.Tasks;

namespace WebApplicationOrders.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductProxy _proxy;

        public ProductsController(IProductProxy proxy)
        {
            _proxy = proxy;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _proxy.GetAllAsync();
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductName,SupplierId,UnitPrice,Package,IsDiscontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                return await HandleActionAsync(async () =>
                {
                    var result = await _proxy.CreateAsync(product);
                    if (result == null)
                    {
                        return RedirectToAction("Error", new { message = "El producto con el mismo nombre ya existe para el proveedor seleccionado." });
                    }
                    return RedirectToAction(nameof(Index));
                }, product);
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _proxy.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,SupplierId,UnitPrice,Package,IsDiscontinued")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return await HandleActionAsync(async () =>
                {
                    var result = await _proxy.UpdateAsync(id, product);
                    if (!result)
                    {
                        return RedirectToAction("Error", new { message = "No se puede realizar la edición porque hay duplicidad de nombre con otro producto del mismo proveedor." });
                    }
                    return RedirectToAction(nameof(Index));
                }, product);
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _proxy.GetByIdAsync(id);
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
            return await HandleActionAsync(async () =>
            {
                var result = await _proxy.DeleteAsync(id);
                if (!result)
                {
                    return RedirectToAction("Error", new { message = "No se puede eliminar el producto porque está asociado a pedidos." });
                }
                return RedirectToAction(nameof(Index));
            });
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _proxy.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Error view handler
        public IActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }

        private async Task<IActionResult> HandleActionAsync(Func<Task<IActionResult>> action, object model = null)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }
    }
}
