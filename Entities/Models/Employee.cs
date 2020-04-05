using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestAPI.Contracts;

namespace TestAPI.Models
{
    public partial class Employee: IEntity
    {
        public Employee()
        {
            PurchaseOrder = new HashSet<PurchaseOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        [Phone]
        public int? PhoneNumber { get; set; }
        public decimal? Salary { get; set; }

        public virtual ICollection<PurchaseOrder> PurchaseOrder { get; set; }
    }
}
