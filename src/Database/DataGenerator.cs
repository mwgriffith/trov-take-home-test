using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Database.Entities;
using System.Linq;

namespace Database
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new APIContext(serviceProvider.GetRequiredService<DbContextOptions<APIContext>>()))
            {
                // Only seed the db if we don't have anything in it.
                if (context.Items.Any())
                {
                    return;
                }

                context.Items.Add(new Item { Name = "Item 1", Description = "Item 1 Description", Price = 1, Quantity = 5 });
                context.Items.Add(new Item { Name = "Item 2", Description = "Item 2 Description", Price = 2, Quantity = 3 });
                context.Items.Add(new Item { Name = "Item 3", Description = "Item 3 Description", Price = 3, Quantity = 6 });
                context.Items.Add(new Item { Name = "Item 4", Description = "Item 4 Description", Price = 4, Quantity = 7 });
                context.SaveChanges();
            }
        }
    }
}
