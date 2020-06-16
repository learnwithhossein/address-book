using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persist
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
