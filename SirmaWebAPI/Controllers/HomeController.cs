using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using SirmaWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SirmaWebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileService _fileService;

        public HomeController(ILogger<HomeController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            if (file.Length > 0)
            {
                string[] lines = new string[]{ };
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploud");
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(stream);
                    }
                    lines = System.IO.File.ReadAllLines(Path.Combine(filePath, file.FileName));
                }
                catch (Exception ex)
                {
                    string fileTest = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploud", "Emp.txt");
                    lines = System.IO.File.ReadAllLines(fileTest);
                }
                ViewBag.GroupedEmps = _fileService.getData(lines);
            }

            return View("Index");
        }
    }
}
