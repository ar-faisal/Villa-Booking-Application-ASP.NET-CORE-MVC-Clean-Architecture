using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var Amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(Amenities);
        }

        public IActionResult Create()
        {

            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem  //SelectListItem used for Select list type, as it is selecting the list into select and it uses the Microsoft.aspnetcore.mvc.rendering
                {

                    Text = u.Name,
                    Value = u.Id.ToString()
                })

            };



            return View(amenityVM);

        }
        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
            ModelState.Remove("Amenity.Villa"); //Removing ModelState Validation for villa
            

            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity has been created successfully.";
                return RedirectToAction("Index", "Amenity"); //explicitly we write the Villa controller or we writre simply "Index"

            }
            
            

            

            TempData["error"] = "The Amenity cannot be Created.";

            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem  //SelectListItem used for Select list type, as it is selecting the list into select and it uses the Microsoft.aspnetcore.mvc.rendering
            {

                Text = u.Name,
                Value = u.Id.ToString()
            });



            return View(obj);


        }

        public IActionResult Update(int AmenityId)
        {

            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem  //SelectListItem used for Select list type, as it is selecting the list into select and it uses the Microsoft.aspnetcore.mvc.rendering
                {

                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == AmenityId)

            };


            

            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");

            }


            else
            {
                return View(amenityVM);
            }


        }

        [HttpPost]
        public IActionResult Update(AmenityVM obj)
        {
            ModelState.Remove("VillaNumber.Villa"); //Removing ModelState Validation for villa


            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity has been updated successfully.";
                return RedirectToAction("Index", "Amenity"); //explicitly we write the Villa controller or we writre simply "Index"

            }


            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem  //SelectListItem used for Select list type, as it is selecting the list into select and it uses the Microsoft.aspnetcore.mvc.rendering
            {

                Text = u.Name,
                Value = u.Id.ToString()
            });



            return View(obj);


        }

        public IActionResult Delete(int AmenityId)
        {

            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem  //SelectListItem used for Select list type, as it is selecting the list into select and it uses the Microsoft.aspnetcore.mvc.rendering
                {

                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == AmenityId)

            };

            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");

            }


            else
            {
                return View(amenityVM);
            }


        }
        [HttpPost]
        public IActionResult Delete(AmenityVM obj)
        {
            Amenity? objFromDb = _unitOfWork.Amenity.Get(u => u.Id == obj.Amenity.Id);
            if (objFromDb != null)
            {
                _unitOfWork.Amenity.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity has been deleted successfully.";
                return RedirectToAction("Index"); //explicitly we write the Villa controller or we writre simply "Index"

            }
            else
            {
                TempData["error"] = "The Amenity could not be deleted.";
                return View();

            }
        }

    }
}
