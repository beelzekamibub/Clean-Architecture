using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        [DisplayName("Villa Name")]
        public required string Name { get; set; }
        public string? Description { get; set; }

        [DisplayName("Price per night")]
        public double Price { get; set; }

        public int Sqft { get; set; }

        public int Occupancy { get; set; }

        [DisplayName("Image Url")]
        public string? ImageUrl { get; set; }

        public DateTime? Created_Date { get; set; }
        public DateTime? Updated_Date { get;set; }

    }
}
