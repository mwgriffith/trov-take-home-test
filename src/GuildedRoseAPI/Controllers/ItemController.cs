using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using GuildedRoseAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace GuildedRoseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        public readonly IItemService _itemService;


        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }



        /// <summary>
        /// Gets a list of the items still in inventory.
        /// </summary>
        /// <returns>An Ienumerable of ItemModel.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemModel>>> GetInventory()
        {
            var items = await _itemService.GetInventory();
            return Ok(items.Select(s => ItemModel.ToItemModel(s)));
        }


        /// <summary>
        /// Buys the specified item from it's id.
        /// </summary>
        /// <param name="id">The id of the item to be bought.</param>
        /// <returns>True if successful, false if not.</returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Boolean>> BuyItem(long id)
        {
            var result = await _itemService.BuyItem(id);
            return Ok(result);
        }
    }
}