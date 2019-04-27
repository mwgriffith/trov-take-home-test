using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;

namespace BusinessLogic
{
    public interface IItemService
    {
        Task<bool> BuyItem(long Id);
        Task<IEnumerable<Item>> GetInventory();
    }
}