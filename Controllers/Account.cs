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
        public IActionResult Edit( int id)
        {
            Galleries galleries1 = mercyContext1.Galleries.Find(id);

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
                Galleries galleries = mercyContext1.Galleries.Find(viewEditModel.Id);
                galleries.Id = viewEditModel.Id;
                galleries.Name = viewEditModel.Name;
                if (viewEditModel.Photo !=null)
                {
                    if (viewEditModel.CurrentPhoto !=null)
                    {
                      string file=  Path.Combine(iwebHostEnvironment.WebRootPath, "images", 
                          viewEditModel.CurrentPhoto);
                        //if (file!=null)
                        //{
                            System.IO.File.Delete(file);
                        //}
                        //else
                        //{
                        //    return View("Picture already Exist.");
                        //}
                        
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


        //Delete product
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var remove = mercyContext1.Galleries.FirstOrDefault(q=>q.Id==id);
                       
            return View(remove);
        }


        //Delete product
       [HttpPost]
        public IActionResult Delete( DeleteModel deleteModel, ViewEditModel viewEditModel, int id)
        {
            Galleries remove = mercyContext1.Galleries.Find(id);


            DeleteModel deleteModel1 = new DeleteModel
            {
                Id = remove.Id,
                Name = remove.Name,
                DeletePhoto = remove.Photo
            };

            string upload = Path.Combine(iwebHostEnvironment.WebRootPath, "images");
          string  uniqueName = Guid.NewGuid().ToString() + "_" + deleteModel.Photo.FileName;
            string filePath = Path.Combine(upload, uniqueName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                deleteModel.Photo.CopyTo(fileStream);
            }

            var sd = Path.Combine(iwebHostEnvironment.WebRootPath, "images", deleteModel.DeletePhoto);
            if(System.IO.File.Exists(sd))
            System.IO.File.Delete(sd);
            mercyContext1.Galleries.Remove(remove);
            mercyContext1.SaveChanges();
            
            RedirectToAction("UploadInfo", "Account");
           
            return View();
        }

        //List of uploaded info
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
    }
}
