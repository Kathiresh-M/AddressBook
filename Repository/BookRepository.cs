using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class BookRepository : DbContext
    {
        public BookRepository(DbContextOptions<BookRepository> options):
            base(options)
        {

        }

        public DbSet<Profiles> Profile { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Phone> Phone { get; set; }
        public DbSet<Assert> Assert { get; set; }
        public DbSet<RefSet> RefSet { get; set; }
        public DbSet<RefTerm> RefTerm { get; set; }
        public DbSet<RefSetTerm> RefSetTerm { get; set; }
    }
}
