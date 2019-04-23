using Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
    }
}
