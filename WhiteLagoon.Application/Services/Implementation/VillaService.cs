using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;
using Microsoft.Extensions.Hosting;

namespace WhiteLagoon.Application.Services.Implementation
{
    public class VillaService : IVillaService    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostEnvironment _hostEnvironment;

        public VillaService(IUnitOfWork unitOfWork, IHostEnvironment HostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = HostEnvironment;

        }



        public void CreateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"images\VillaImage");

                using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                villa.Image.CopyTo(fileStream);

                villa.ImageUrl = @"\images\VillaImage\" + fileName;
            }
            else
            {
                villa.ImageUrl = "https://placehold.co/600x400";
            }

            _unitOfWork.Villa.Add(villa);
            _unitOfWork.Save();
        }

        public bool DeleteVilla(int id)
        {
            try
            {
                Villa? objFromDb = _unitOfWork.Villa.Get(u => u.Id == id);
                if (objFromDb is not null)
                {
                    if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_hostEnvironment.ContentRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    _unitOfWork.Villa.Remove(objFromDb);
                    _unitOfWork.Save();

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Villa> GetAllVillas()
        {
            return _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");
        }

        public Villa GetVillaById(int id)
        {
            return _unitOfWork.Villa.Get(u => u.Id == id, includeProperties: "VillaAmenity");
        }

        

        public void UpdateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"images\VillaImage");

                if (!string.IsNullOrEmpty(villa.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_hostEnvironment.ContentRootPath, villa.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                villa.Image.CopyTo(fileStream);

                villa.ImageUrl = @"\images\VillaImage\" + fileName;
            }

            _unitOfWork.Villa.Update(villa);
            _unitOfWork.Save();
        }
    }
}
