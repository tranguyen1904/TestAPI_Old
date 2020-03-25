using System;
using System.Collections.Generic;

namespace TestAPI.Models
{
    public partial class PurchaseOrder
    {
        public PurchaseOrder()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
