using AddressBook.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Persist
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(x => x.Contacts)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
