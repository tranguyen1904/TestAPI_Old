using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestAPI.Contracts;

namespace TestAPI.ViewModels
{
    public class EmployeeViewModel: IEntity
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
