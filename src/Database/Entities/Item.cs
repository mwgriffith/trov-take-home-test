using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    /// <summary>
    /// The item that is sold by the Guilded Rose
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The database generated id for the item.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        /// The quantity of the items available.  Regularly this would probably come from somewhere else, but throwing it in here to make it easier.
        /// </summary>
        public int Quantity { get; set; }
    }
}
