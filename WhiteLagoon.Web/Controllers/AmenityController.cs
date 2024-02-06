using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IAmenityService _amenityService;
        private readonly IVillaService _villaService;

        public AmenityController(IAmenityService amenityService, IVillaService villaService)
        {
            _amenityService = amenityService;
            _villaService = villaService;
        }
        public IActionResult Index()
        {
            var amenities = _amenityService.GetAllAmenities();
            return View(amenities);
        }

        public IActionResult Create()
        {

            AmenityVM amenityVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
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
                _amenityService.CreateAmenity(obj.Amenity);
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }





            TempData["error"] = "The Amenity cannot be Created.";

            obj.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
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
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(AmenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);


        }

        [HttpPost]
        public IActionResult Update(AmenityVM obj)
        {
            ModelState.Remove("VillaNumber.Villa"); //Removing ModelState Validation for villa


            if (ModelState.IsValid)
            {
                _amenityService.UpdateAmenity(obj.Amenity);
                TempData["success"] = "The amenity has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            obj.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
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
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(AmenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);


        }
        [HttpPost]
        public IActionResult Delete(AmenityVM obj)
        {
            Amenity? objFromDb = _amenityService.GetAmenityById(obj.Amenity.Id);
            if (objFromDb is not null)
            {
                _amenityService.DeleteAmenity(objFromDb.Id);
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The amenity could not be deleted.";
            return View();
        }

    }
}
