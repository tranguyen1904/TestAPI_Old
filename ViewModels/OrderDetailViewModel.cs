using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestAPI.ViewModels
{
    public class OrderDetailViewModel
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int? Quantity { get; set; }

    }
}
