using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World_Shopping.Models.Models;

namespace World_Shopping.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public decimal TotalPrice;

        public IEnumerable<ShoppingCart> CartsList { get; set; }
        public decimal TotalCarts { get; set; }
        public  OrderHeader  OrderHeader { get; set; }
    }
}
