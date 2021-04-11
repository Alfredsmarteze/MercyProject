using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MercyProject.Models;
using MercyProject.Data;
using MercyProject.ViewModel;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MercyProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MercyContext _mercyContext;
       // private readonly IHostingEnvironment _hostingEnvironment1;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(ILogger<HomeController> logger, MercyContext mercyContext
            , IWebHostEnvironment webHostEnvironment)
        //IHostingEnvironment hostingEnvironment
        {
            _logger = logger;
            _mercyContext = mercyContext;
          //  _hostingEnvironment1 = hostingEnvironment;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult YourMessage()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YourMessage(HowCanWeHelpYou howCanWeHelpYou)
        {
            if (ModelState.IsValid)
            {
                _mercyContext.howCanWeHelpYou.Add(howCanWeHelpYou);
                await _mercyContext.SaveChangesAsync();
                ModelState.Clear();
                ViewData["Message"] = "Thank you, we have recieved your message.";
                RedirectToAction("Index", "Home");

            }
            else
            {

                ModelState.AddModelError(string.Empty, "");

                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Gallery()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Gallery( Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                uniqueFileName = MethodExtraction(gallery, uniqueFileName);
                Galleries galleris = new Galleries
                {
                    Id = gallery.Id,
                    Name = gallery.Name,
                    Photo = uniqueFileName,
                };
                _mercyContext.Add(galleris);
                _mercyContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        private string MethodExtraction(Gallery gallery, string uniqueFileName)
        {
            if (gallery.Photo != null)
            {
                string upload = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + gallery.Photo.FileName;
                string filePath = Path.Combine(upload, uniqueFileName);
                gallery.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return uniqueFileName;
        }

        [HttpGet]
        public IActionResult UploadDetails(int? id)
        {
            var asd=this._mercyContext.Galleries.FirstOrDefault(s => s.Id == id);
            return View("UploadDetails",asd);
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
    }
}
