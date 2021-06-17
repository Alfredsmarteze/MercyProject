using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MercyProject.Data;
using MercyProject.Models;
using MercyProject.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MercyProject.Controllers
{
    public class Account : Controller
    {
        private readonly MercyContext mercyContext1;
        private readonly IWebHostEnvironment iwebHostEnvironment;

        public Account(MercyContext mercyContext, IWebHostEnvironment webHostEnvironment)
        {
            this.mercyContext1 = mercyContext;
            this.iwebHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult ClientsInfo()
        {
            return View();
        }
       
        //Get a product to edit
        [HttpGet]
        public async Task<IActionResult> Edit( int id)
        {
            /*Galleries*/var  galleries1 =  mercyContext1.Galleries.Find(id);

            ViewEditModel viewEditModel = new ViewEditModel
            {
                Id = galleries1.Id,
                Name = galleries1.Name,
                CurrentPhoto = galleries1.Photo,
            };

            return View(viewEditModel);
        }

        // Update product
        [HttpPost] 
        public IActionResult Edit(ViewEditModel viewEditModel)
        {
            
            if (ModelState.IsValid)
            {
                var galleries = mercyContext1.Galleries.Find(viewEditModel.Id);
                galleries.Id = viewEditModel.Id;
                galleries.Name = viewEditModel.Name;
                if (viewEditModel.Photo !=null)
                {
                    if (viewEditModel.CurrentPhoto !=null)
                    {
                      string file=  Path.Combine(iwebHostEnvironment.WebRootPath, "images", 
                          viewEditModel.CurrentPhoto);
                        if (file != null)
                        {
                            System.IO.File.Delete(file);
                    }
                    else
                    {
                        return View("Picture already Exist.");
                    }

                }
                    galleries.Photo= NewMethod(viewEditModel);
                }
                mercyContext1.Update(galleries);
                mercyContext1.SaveChanges();

                return RedirectToAction("UploadInfo","Account");
            }
            return View();
        }

        // Quick action and refactory method
        private string NewMethod(ViewEditModel viewEditModel)
        {
            string uniqueFileName = null;
            if (viewEditModel.Photo != null)
            {
                string upload = Path.Combine(iwebHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + viewEditModel.Photo.FileName;
                string filePath = Path.Combine(upload, uniqueFileName);
                using (var fileStream= new FileStream(filePath, FileMode.Create))
                {
                    viewEditModel.Photo.CopyTo(fileStream);
                }
                
            }

            return uniqueFileName;
        }


       

        [HttpGet]
        public IActionResult Gallery()
        {
            return View();
        }

        //Upload a product
        [HttpPost]
        public IActionResult Gallery(Gallery gallery, int id)
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
                mercyContext1.Add(galleris);
                mercyContext1.SaveChanges();

                return RedirectToAction("UploadInfo", "Account", new {id=gallery.Id });
            }
            return View();
        }

        //Quick action and refactory method
        private string MethodExtraction(Gallery gallery, string uniqueFileName)
        {
            if (gallery.Photo != null)
            {
                string upload = Path.Combine(iwebHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + gallery.Photo.FileName;
                string filePath = Path.Combine(upload, uniqueFileName);
                using (var fileStream= new FileStream(filePath, FileMode.Create))
                {
                    gallery.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }


        //Get product to delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var remove = mercyContext1.Galleries.FirstOrDefault(q => q.Id == id);

            return View(remove);
        }

        //Delete product
        [HttpPost]
        public IActionResult Delete(DeleteModel deletemodel, Gallery galleries, ViewEditModel viewEditModel, int id)
        { 
                        

            var remove = mercyContext1.Galleries.Find(id);
            string sd =Path.Combine(iwebHostEnvironment.WebRootPath,"images", remove.Photo);
            if(sd !=null)
            {
                System.IO.File.Delete(sd);
            }
          

            mercyContext1.Galleries.Remove(remove);
            mercyContext1.SaveChanges();

          return  RedirectToAction("UploadInfo");

        }

       //List of product
        public IActionResult UploadInfo()
        {
           var getList= this.mercyContext1.Galleries.ToList();
            if (getList !=null)
            {
                return View(getList);
            }
            else
            {
                return View();
            }
            
        }

        
        //Details of a product
        public IActionResult UploadsDetails( Galleries viewDetails, int? id)
        {
            var filename = mercyContext1.ViewDetails.FirstOrDefault(s => s.Id == id);
            if (filename != null)
            {
                return View("UploadsDetails", filename);
            }
            else
            {
                return View();
            }
          
        }

        public async Task<IActionResult> AddProduct(Products products)
        {
            if (ModelState.IsValid)
            {
                mercyContext1.Products.Add(products);
                await mercyContext1.SaveChangesAsync();
             return   RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
