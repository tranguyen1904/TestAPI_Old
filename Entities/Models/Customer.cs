using System;
using System.Collections.Generic;
using TestAPI.Contracts;

namespace TestAPI.Models
{
    public partial class Customer: IEntity
    {
        public Customer()
        {
            PurchaseOrder = new HashSet<PurchaseOrder>();
            
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int? PhoneNumber { get; set; }
        public string Address { get; set; }

        public virtual ICollection<PurchaseOrder> PurchaseOrder { get; set; }
    }
}
