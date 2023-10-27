﻿using HotelBooking.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBooking.Web.ViewModels
{
    public class AmenityVM
    {
        public Amenity? Amenity { get; set; }
        public IEnumerable<SelectListItem>? VillaList { get; set; }
    }
}
