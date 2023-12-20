using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Infrastructure;
using WhiteLagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            //Before we did, var villaNumbers = _db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem  //SelectListItem used for Select list type, as it is selecting the list into select and it uses the Microsoft.aspnetcore.mvc.rendering
                {

                    Text = u.Name,
                    Value = u.Id.ToString()
                })

            };

            
            
            return View(villaNumberVM);

        }
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            ModelState.Remove("VillaNumber.Villa"); //Removing ModelState Validation for villa
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been created successfully.";
                return RedirectToAction("Index", "VillaNumber"); //explicitly we write the Villa controller or we writre simply "Index"

            }
            if (roomNumberExists)
            {
                TempData["error"] = "The Villa Number Already exists.";

            }



            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem  //SelectListItem used for Select list type, as it is selecting the list into select and it uses the Microsoft.aspnetcore.mvc.rendering
            {

                Text = u.Name,
                Value = u.Id.ToString()
            });

            

            return View(obj);

            
        }
        public IActionResult Update(int VillaNumberId)
        {            

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem  //SelectListItem used for Select list type, as it is selecting the list into select and it uses the Microsoft.aspnetcore.mvc.rendering
                {

                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == VillaNumberId)

            };

            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");

            }


            else
            {
                return View(villaNumberVM);
            }


        }
        
        [HttpPost]
        public IActionResult Update(VillaNumberVM obj)
        {
            ModelState.Remove("VillaNumber.Villa"); //Removing ModelState Validation for villa
            

            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been updated successfully.";
                return RedirectToAction("Index", "VillaNumber"); //explicitly we write the Villa controller or we writre simply "Index"

            }
            

            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem  //SelectListItem used for Select list type, as it is selecting the list into select and it uses the Microsoft.aspnetcore.mvc.rendering
            {

                Text = u.Name,
                Value = u.Id.ToString()
            });



            return View(obj);


        }
        public IActionResult Delete(int VillaNumberId)
        {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem  //SelectListItem used for Select list type, as it is selecting the list into select and it uses the Microsoft.aspnetcore.mvc.rendering
                {

                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == VillaNumberId)

            };

            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");

            }


            else
            {
                return View(villaNumberVM);
            }


        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM obj)
        {
            VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            if (objFromDb != null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been deleted successfully.";
                return RedirectToAction("Index"); //explicitly we write the Villa controller or we writre simply "Index"

            }
            else
            {
                TempData["error"] = "The Villa Number could not be deleted.";
                return View();

            }
        }
    }
}
