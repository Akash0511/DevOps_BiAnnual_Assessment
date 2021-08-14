using Ecommerce.Contracts;
using Ecommerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce_tests
{
    public class ShoppingCartServiceFake : IShoppingCartService
    {
        private readonly List<ShoppingItem> _shoppingCart;

        public ShoppingCartServiceFake()
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

        public IEnumerable<ShoppingItem> GetAllItems()
        {
            return _shoppingCart;
        }

        public ShoppingItem Add(ShoppingItem newItem)
        {
            newItem.Id = _shoppingCart.Count + 1;
            _shoppingCart.Add(newItem);
            return newItem;
        }

        public ShoppingItem GetById(int id)
        {
            return _shoppingCart.Where(a => a.Id == id)
                .FirstOrDefault();
        }
    }
}
