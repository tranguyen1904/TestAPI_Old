using System;
using System.Collections.Generic;
using TestAPI.Contracts;

namespace TestAPI.Models
{
    public partial class OrderDetail: IEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }

        public virtual PurchaseOrder Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
