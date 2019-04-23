using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic
{
    /// <summary>
    /// Houses the business logic for all item operations.
    /// </summary>
    public class ItemService
    {
        public readonly APIContext _apiContext;

        public ItemService(APIContext apicontext)
        {
            _apiContext = apicontext;
        }


        /// <summary>
        /// Gets the inventory of available items.
        /// </summary>
        /// <returns>A List of items that are available.</returns>
        public async Task<IEnumerable<Item>> GetInventory()
        {
            return await _apiContext.Items.Where(w => w.Quantity > 0).ToListAsync();
        }



        /// <summary>
        /// Buys the item.
        /// </summary>
        /// <param name="Id">The id of the item to buy.</param>
        /// <returns>true if the purchase was successful, false otherwise.</returns>
        public async Task<Boolean> BuyItem(long Id)
        {
            // TODO add exceptions and transactions.
            var itemToBuy = await _apiContext.Items.FindAsync(Id);

            if (itemToBuy == null)
            {
                // Item to buy not found.
                return false; 
            }

            if (itemToBuy.Quantity <= 0)
            {
                // No more quanity to buy.
                return false;
            }

            itemToBuy.Quantity--;
            _apiContext.Entry(itemToBuy).State = EntityState.Modified;
            await _apiContext.SaveChangesAsync();
            return true;
        }


    }
}
