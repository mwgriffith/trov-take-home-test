using BusinessLogic;

using Database.Entities;

using GuildedRoseAPI.Controllers;
using GuildedRoseAPI.Models;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{

    /// <summary>
    /// This class tests the ItemController in the GuildedRoseAPI.
    /// </summary>
    [TestFixture]
    public class ItemControllerTests
    {

        /// <summary>
        /// Creates a hard coded list of Items for mocking purposes.
        /// </summary>
        /// <returns>An IEnumerable of Item.</returns>
        private IEnumerable<Item> GetDBItems()
        {
            return new List<Item>() {
                new Item { Name = "Item 1", Description = "Item 1 Description", Price = 1, Quantity = 5 },
                new Item { Name = "Item 2", Description = "Item 2 Description", Price = 2, Quantity = 0 },
                new Item { Name = "Item 3", Description = "Item 3 Description", Price = 3, Quantity = 6 },
                new Item { Name = "Item 4", Description = "Item 4 Description", Price = 4, Quantity = 7 }
            };
        }


        /// <summary>
        /// Tests the GetInventory call.
        /// </summary>        
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


        /// <summary>
        /// Tests the BuyItem call.
        /// </summary>        
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
