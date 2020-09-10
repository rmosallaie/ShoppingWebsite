using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWebsite.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public double TotalPrice { get; set; }
    }
}
