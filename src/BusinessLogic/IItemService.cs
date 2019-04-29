using Database.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IItemService
    {
        Task<bool> BuyItem(long Id);
        Task<IEnumerable<Item>> GetInventory();
    }
}