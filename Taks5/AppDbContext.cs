using Microsoft.EntityFrameworkCore;
using Taks5.Entities;
using Taks5.Models;

namespace Taks5
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        public DbSet<UserAccount> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Taks5.Models.RegistrationViewModel> RegistrationViewModel { get; set; } = default!;
    }
}
