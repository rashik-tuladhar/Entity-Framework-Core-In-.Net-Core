using Microsoft.EntityFrameworkCore;
using WebApi.Shared.Model;

namespace WebApi.DataAccess.EntityFrameworkCore.DataModel
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Contact> ContactDetails { get; set; }
    }
}
