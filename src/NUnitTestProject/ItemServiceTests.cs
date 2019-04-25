using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Database;
using Database.Entities;
using BusinessLogic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;

namespace Tests
{
    [TestFixture]
    public class ItemServiceTests
    {
                

        private DbContextOptions<APIContext> CreateInMemoryDB()
        {
            var options = new DbContextOptionsBuilder<APIContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;            

            // Seed test inventory into the in memory db.
            using (var context = new APIContext(options))
            {
                context.Items.Add(new Item { Name = "Item 1", Description = "Item 1 Description", Price = 1, Quantity = 5 });
                context.Items.Add(new Item { Name = "Item 2", Description = "Item 2 Description", Price = 2, Quantity = 0 });
                context.Items.Add(new Item { Name = "Item 3", Description = "Item 3 Description", Price = 3, Quantity = 6 });
                context.Items.Add(new Item { Name = "Item 4", Description = "Item 4 Description", Price = 4, Quantity = 7 });
                context.SaveChanges();
            }

            return options;
        }



        [SetUp]
        public void Setup()
        {
            
        }


        /// <summary>
        /// Tests the GetInventory method of ItemService in the BusinessLogic project.
        /// </summary>
        [Test]        
        public void Test_ItemService_GetInventory()
        {
            var options = CreateInMemoryDB();
            
            using (var testContext = new APIContext(options))
            {
                var itemService = new ItemService(testContext);
                var result = itemService.GetInventory().Result.ToList();

                Assert.That(result, Is.All.Not.Null);
                Assert.That(result.Select(s => s.Quantity), Is.All.GreaterThan(0));
                Assert.That(result, Is.Unique);
                Assert.AreEqual(3, result.Count());
            }
        }


        /// <summary>
        /// Check to see if the Basic BuyItem functionality works.
        /// </summary>
        [Test]
        public void Test_ItemService_BuyItem_CheckGoodBuy()
        {
            var options = CreateInMemoryDB();

            using (var testContext = new APIContext(options))
            {
                var itemService = new ItemService(testContext);
                var result = itemService.BuyItem(1).Result;

                Assert.AreEqual(result, true);
                var modifiedItem = testContext.Items.FirstOrDefault(f => f.Id == 1);
                Assert.AreEqual(4, modifiedItem.Quantity);
            }
        }



        /// <summary>
        /// Check to see if the Check for the item not found works in BuyItem.
        /// </summary>
        [Test]
        public void Test_ItemService_BuyItem_CheckNotFoundItem()
        {
            var options = CreateInMemoryDB();

            using (var testContext = new APIContext(options))
            {
                var itemService = new ItemService(testContext);
                var result = itemService.BuyItem(-1).Result;

                Assert.AreEqual(result, false);                
            }
        }


        /// <summary>
        /// Check to see if checking for no quantity works in BuyItem.
        /// </summary>
        [Test]
        public void Test_ItemService_BuyItem_CheckNoQuantity()
        {
            var options = CreateInMemoryDB();

            using (var testContext = new APIContext(options))
            {
                var itemService = new ItemService(testContext);
                var result = itemService.BuyItem(2).Result;

                Assert.AreEqual(result, false);
            }
        }
    }
}