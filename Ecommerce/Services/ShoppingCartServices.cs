using Ecommerce.Contracts;
using Ecommerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class ShoppingCartServices : IShoppingCartService
    {
        private readonly List<ShoppingItem> _shoppingCart;

        public ShoppingCartServices()
        {
            _shoppingCart = new List<ShoppingItem>()
            {
                new ShoppingItem() { Id = 1,
                    Name = "Orange Juice", Manufacturer="Orange Tree", Price = 5.00M },
                new ShoppingItem() { Id = 2,
                    Name = "Diary Milk", Manufacturer="Mad Cow", Price = 4.00M },
                new ShoppingItem() { Id = 3,
                    Name = "Frozen Pizza", Manufacturer="Uncle Mickey's", Price = 12.00M }
            };
        }
        public ShoppingItem Add(ShoppingItem newItem)
        {
            newItem.Id = _shoppingCart.Count + 1;
            _shoppingCart.Add(newItem);
            return newItem;
        }

        public IEnumerable<ShoppingItem> GetAllItems()
        {
            return _shoppingCart;
        }

        public ShoppingItem GetById(int id)
        {
            return _shoppingCart.Where(a => a.Id == id)
                .FirstOrDefault();
        }
    }
}
