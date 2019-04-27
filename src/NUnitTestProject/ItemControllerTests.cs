using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using BusinessLogic;
using Database.Entities;
using GuildedRoseAPI.Controllers;
using GuildedRoseAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class ItemControllerTests
    {

        private IEnumerable<Item> GetDBItems()
        {
            return new List<Item>() {
                new Item { Name = "Item 1", Description = "Item 1 Description", Price = 1, Quantity = 5 },
                new Item { Name = "Item 2", Description = "Item 2 Description", Price = 2, Quantity = 0 },
                new Item { Name = "Item 3", Description = "Item 3 Description", Price = 3, Quantity = 6 },
                new Item { Name = "Item 4", Description = "Item 4 Description", Price = 4, Quantity = 7 }
            };
        }

        [Test]
        public async Task Test_GetInventory()
        {
            var mockItemService = new Mock<IItemService>();
            mockItemService.Setup(iservice => iservice.GetInventory())
                .ReturnsAsync(GetDBItems);

            var controller = new ItemController(mockItemService.Object);

            var actionResult = await controller.GetInventory();
            var result = actionResult.Result as OkObjectResult;
            
            Assert.IsNotNull(actionResult);            
            Assert.IsInstanceOf<ActionResult<IEnumerable<ItemModel>>>(actionResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);            
        }


        [Test]
        public async Task Test_BuyItem()
        {
            var mockItemService = new Mock<IItemService>();
            mockItemService.Setup(iservice => iservice.BuyItem(1))
                .ReturnsAsync(true);

            var controller = new ItemController(mockItemService.Object);


            var actionResult = await controller.BuyItem(1);
            var result = actionResult.Result as OkObjectResult;

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Boolean>>(actionResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
