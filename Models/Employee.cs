using System;
using System.Collections.Generic;

namespace TestAPI.Models
{
    public partial class Employee
    {
        public Employee()
        {
            PurchaseOrder = new HashSet<PurchaseOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int? PhoneNumber { get; set; }
        public decimal? Salary { get; set; }

        public virtual ICollection<PurchaseOrder> PurchaseOrder { get; set; }
    }
}
