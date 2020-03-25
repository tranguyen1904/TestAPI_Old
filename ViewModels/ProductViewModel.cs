using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestAPI.ViewModels
{
    public class ProductViewModel
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public decimal? UnitPrice { get; set; }

    }
}
