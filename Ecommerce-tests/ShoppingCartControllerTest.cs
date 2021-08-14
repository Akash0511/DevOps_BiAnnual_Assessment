using Ecommerce.Contracts;
using Ecommerce.Controllers;
using Ecommerce.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ecommerce_tests
{
    public class ShoppingCartControllerTest
    {
        ShoppingCartController _controller;
        IShoppingCartService _service;

        public ShoppingCartControllerTest()
        {
            _service = new ShoppingCartServiceFake();
            _controller = new ShoppingCartController(_service);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.Get().Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<ShoppingItem>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public void GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = _controller.Get(4);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.Get(1);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var okResult = _controller.Get(1).Result as OkObjectResult;

            // Assert
            Assert.IsType<ShoppingItem>(okResult.Value);
            Assert.Equal(1, (okResult.Value as ShoppingItem).Id);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new ShoppingItem()
            {
                Manufacturer = "Guinness",
                Price = 12.00M
            };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse = _controller.Post(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            ShoppingItem testItem = new ShoppingItem()
            {
                Name = "Guinness Original 6 Pack",
                Manufacturer = "Guinness",
                Price = 12.00M
            };

            // Act
            var createdResponse = _controller.Post(testItem);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var testItem = new ShoppingItem()
            {
                Name = "Guinness Original 6 Pack",
                Manufacturer = "Guinness",
                Price = 12.00M
            };

            // Act
            var createdResponse = _controller.Post(testItem) as CreatedAtActionResult;
            var item = createdResponse.Value as ShoppingItem;

            // Assert
            Assert.IsType<ShoppingItem>(item);
            Assert.Equal("Guinness Original 6 Pack", item.Name);
        }
    }
}
