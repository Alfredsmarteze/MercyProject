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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(ILogger<HomeController> logger, MercyContext mercyContext
            , IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _mercyContext = mercyContext;
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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> YourMessage(HowCanWeHelpYou howCanWeHelpYou)
        {
            if (ModelState.IsValid)
            {
                _mercyContext.howCanWeHelpYou.Add(howCanWeHelpYou);
                await _mercyContext.SaveChangesAsync();
                ModelState.Clear();
                ViewData["Message"] = "Thank you, we have recieved your message.";
             return   RedirectToAction("Index", "Home");

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
            var asd=this._mercyContext.Galleries.ToList();
            return View(asd);
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
