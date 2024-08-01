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

        //Crete 
        //GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Firstmane,LastName,City,Country,Phone")]Customer customer)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    var result = await _proxy.CreateAsync(customer);
                    if (result != null) 
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
    }
}
