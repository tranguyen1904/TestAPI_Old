using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestAPI.ViewModels
{
    public class EmployeeViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string Gender { get; set; }
        public int? PhoneNumber { get; set; }
        public decimal? Salary { get; set; }

    }
}
