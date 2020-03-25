using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestAPI.ViewModels
{
    public class PurchaseOrderViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }

    }
}
