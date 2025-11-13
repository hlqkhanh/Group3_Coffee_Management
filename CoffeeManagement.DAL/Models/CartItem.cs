using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagement.DAL.Models
{
    public class CartItem
    {
        public MenuItem Item { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Item.Price * Quantity;
    }
}
