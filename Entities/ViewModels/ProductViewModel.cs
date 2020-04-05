using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestAPI.Contracts;

namespace TestAPI.ViewModels
{
    public class ProductViewModel: IEntity
    {

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public decimal? UnitPrice { get; set; }

    }
}
