using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        [Display(Name = "Price Per Night")]
        public double Price { get; set; }
        public int Sqft { get; set;}
        public int Occupancy { get; set; }

        [NotMapped] //not mapped used for not making the column in databse
        public IFormFile? Image { get; set; } //iformfile used for storing files

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedTime { get; set; }


        [ValidateNever]
        public IEnumerable<Amenity> VillaAmenity { get; set; }

    }
}
