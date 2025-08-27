using Microsoft.AspNetCore.Mvc;
using ABCRetails.Services;
using ABCRetails.Models;

namespace ABCRetails.Controllers
{
    public class ProductController : Controller
    {
        private readonly IAzureStorageService _storage;
        public ProductController(IAzureStorageService storage) => _storage = storage;

        public async Task<IActionResult> Index()
        {
            var products = await _storage.GetAllAsync<Product>();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create() => View(new Product());

        [HttpPost]
        public async Task<IActionResult> Create(Product model, IFormFile? image)
        {
            if (image != null)
            {
                model.ImageUrl = await _storage.UploadImageAsync(image, model.ProductName?.Replace(" ", "-"));
            }
            await _storage.UpsertAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string partitionKey, string rowKey)
        {
            var products = await _storage.GetAllAsync<Product>();
            var entity = products.FirstOrDefault(c => c.PartitionKey == partitionKey && c.RowKey == rowKey);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product model, IFormFile? image)
        {
            if (image != null)
            {
                model.ImageUrl = await _storage.UploadImageAsync(image, model.ProductName?.Replace(" ", "-"));
            }
            await _storage.UpsertAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            await _storage.DeleteAsync<Product>(partitionKey, rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}