using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestAPI.Contracts;

namespace TestAPI.ViewModels
{
    public class CustomerViewModel: IEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string Gender { get; set; }
        public int? PhoneNumber { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }

    }
}
