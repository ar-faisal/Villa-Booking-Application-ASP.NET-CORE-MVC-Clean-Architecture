using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Infrastructure;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Application.Common.Interfaces;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
            return View(villas);
        }

        public IActionResult Create()
        {

            return View();

        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {   if(obj.Name == obj.Description)
            {
                ModelState.AddModelError("", "THe description cannot exactly match the name.");
                //TO display in name not in summary - ModelState.AddModelError("Name", "THe description cannot exactly match the name.");

            }
            if(ModelState.IsValid)
            {
                if(obj.Image!= null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Images\VillaImage");
                    
                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);
                    obj.ImageUrl = @"\Images\VillaImage\" + fileName;
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been created successfully.";
                return RedirectToAction("Index", "Villa"); //explicitly we write the Villa controller or we writre simply "Index"

            }
            else
            {
                TempData["error"] = "The Villa could not be created.";
                return View();

            }
        }
        public IActionResult Update(int VillaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == VillaId);
            if(obj == null)
            {
                return RedirectToAction("Error", "Home");

            }
            else
            {
                return View(obj);
            }


        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            
            if (ModelState.IsValid)
            {
                if (obj.Image != null)  //when we do the update in villa, if we select the new image, then old image shloud be deleted else, the old image should be used
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImage\" + fileName;
                }


                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been updated successfully.";
                return RedirectToAction("Index", "Villa"); //explicitly we write the Villa controller or we writre simply "Index"

            }
            else
            {
                TempData["error"] = "The Villa could not be updated.";
                return View();

            }
        }
        public IActionResult Delete(int VillaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == VillaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");

            }
            else
            {
                return View(obj);
            }


        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitOfWork.Villa.Get(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been deleted successfully.";
                return RedirectToAction("Index", "Villa"); //explicitly we write the Villa controller or we writre simply "Index"

            }
            else
            {
                TempData["error"] = "The Villa could not be deleted.";
                return View();

            }
        }
    }
}
