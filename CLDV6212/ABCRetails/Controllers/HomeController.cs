using Microsoft.AspNetCore.Mvc;
using ABCRetails.Services;
using ABCRetails.Models.ViewModels;
using ABCRetails.Models;

namespace ABCRetails.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAzureStorageService _storage;
        public HomeController(IAzureStorageService storage) => _storage = storage;

        public async Task<IActionResult> Index()
        {
            var products = await _storage.GetAllAsync<Product>();
            var customers = await _storage.GetAllAsync<Customer>();
            var orders = await _storage.GetAllAsync<Order>();

            var vm = new HomeViewModel
            {
                FeaturedProducts = products.Take(5),
                CustomerCount = customers.Count,
                ProductCount = products.Count,
                OrderCount = orders.Count
            };
            return View(vm);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
    }
}