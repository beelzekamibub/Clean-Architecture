﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Entities
{
	public class VillaNumber
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Display(Name ="Villa Number")]
        public int Villa_Number { get; set; }

		[ForeignKey("Villa")]
		public int VillaId { get; set; }
		
        public Villa Villa { get; set; }

		public string? SpecialDetails { get; set; }
    }
}
