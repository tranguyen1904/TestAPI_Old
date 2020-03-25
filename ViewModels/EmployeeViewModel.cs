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
        public string Name { get; set; }
        public string Gender { get; set; }
        public int? PhoneNumber { get; set; }
        public decimal? Salary { get; set; }

    }
}
