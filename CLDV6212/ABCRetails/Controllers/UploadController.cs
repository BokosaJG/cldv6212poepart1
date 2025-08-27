using Microsoft.AspNetCore.Mvc;
using ABCRetails.Services;
using ABCRetails.Models;

namespace ABCRetails.Controllers
{
    public class UploadController : Controller
    {
        private readonly IAzureStorageService _storage;
        public UploadController(IAzureStorageService storage) => _storage = storage;

        [HttpGet]
        public IActionResult Index() => View(new FileUploadModel());

        [HttpPost]
        public async Task<IActionResult> Index(FileUploadModel model)
        {
            if (model.ProofOfPayment == null)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
                return View(model);
            }
            using var stream = model.ProofOfPayment.OpenReadStream();
            await _storage.UploadContractAsync(model.ProofOfPayment.FileName, stream);
            TempData["Message"] = "File uploaded successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}