using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Entities;

namespace GuildedRoseAPI.Models
{
    public class ItemModel
    {
        /// <summary>
        /// The database generated id for the item.
        /// </summary>        
        public long Id { get; set; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The price of the item.  Regularly I'd use a float or money type.
        /// </summary>
        public int Price { get; set; }


        /// <summary>
        /// Static conversion function to convert the db item to the item model used by the api.
        /// </summary>
        /// <param name="item">The item entity from the db.</param>
        /// <returns>A converted ItemModel.</returns>
        public static ItemModel ToItemModel(Item item)
        {
            return new ItemModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price
            };
        }
    }
}
