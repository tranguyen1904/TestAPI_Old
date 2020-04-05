using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestAPI.Contracts;

namespace TestAPI.ViewModels
{
    public class OrderDetailViewModel: IEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int? Quantity { get; set; }

    }
}
