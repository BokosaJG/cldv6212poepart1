using Microsoft.AspNetCore.Mvc;
using ABCRetails.Services;
using ABCRetails.Models;

namespace ABCRetails.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IAzureStorageService _storage;
        public CustomerController(IAzureStorageService storage) => _storage = storage;

        public async Task<IActionResult> Index()
        {
            var customers = await _storage.GetAllAsync<Customer>();
            return View(customers);
        }

        [HttpGet]
        public IActionResult Create() => View(new Customer());

        [HttpPost]
        public async Task<IActionResult> Create(Customer model)
        {
            if (!ModelState.IsValid) return View(model);
            await _storage.UpsertAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string partitionKey, string rowKey)
        {
            var customers = await _storage.GetAllAsync<Customer>();
            var entity = customers.FirstOrDefault(c => c.PartitionKey == partitionKey && c.RowKey == rowKey);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Customer model)
        {
            if (!ModelState.IsValid) return View(model);
            await _storage.UpsertAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            await _storage.DeleteAsync<Customer>(partitionKey, rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}