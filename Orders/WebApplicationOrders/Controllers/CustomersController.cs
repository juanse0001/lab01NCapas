using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using ProxyService.Interfaces;
using ProxyService;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace WebApplicationOrders.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerProxy _proxy;  
        public CustomersController() 
        {
            this._proxy = new CustomerProxy();
        }
 
        //GET : Customer
        public async Task <IActionResult> Index()
        {
            var customers = await _proxy.GetAllAsync();
            return View(customers);
        }

        //Create 
        //GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,City,Country,Phone")]Customer customer)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    var result = await _proxy.CreateAsync(customer);
                    if (result == null) 
                    {
                        return RedirectToAction("Error", new {message = "El cliente con el mismo nombre y apellido ya existe."});
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) 
                {
                    return RedirectToAction("Error", new { message = ex.Message });
                }
            }
            return View(customer);
        }

        //Edit 
        //GET : Customers/Edit/5
        public async Task<IActionResult> Edit(int Id)
        {
            var customer = await _proxy.GetByIdAsync(Id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        //Details
        //GET: /Customer/Details/5
        public async Task<IActionResult> Details(int Id) 
        {
            var customers = await _proxy.GetByIdAsync(Id);
            if(customers == null)
            {
                return NotFound();
            }
            return View(customers);
        }


        //POST: Customer/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FirstName, LastName, City, Country, Phone")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _proxy.UpdateAsync(id, customer);
                    if (!result)
                    {
                        return RedirectToAction("Error", new { message = "No se puede realizar la edición porque hay duplicidad de nombre con otro cliente." });
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = ex.Message });
                }
            }
            return View(customer);
        }


        //Error
        public IActionResult Error(string message) 
        {
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
