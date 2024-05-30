using InMemorySqliteAssignmentApp.Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace InMemorySqliteAssignmentApp.Infrastructure_Data_
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options){}
        public DbSet<Customer> Customers { get; set; }
    }
}
