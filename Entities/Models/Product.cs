using System;
using System.Collections.Generic;
using TestAPI.Contracts;

namespace TestAPI.Models
{
    public partial class Product: IEntity
    {
        public Product()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? UnitPrice { get; set; }

        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
