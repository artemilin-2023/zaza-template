using ZazaTemplate.Infrastructure.Data.Configurations;
using ZazaTemplate.Infrastructure.Data.Entities;
using ZazaTemplate.Infrastructure.ServiceRegistration.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ZazaTemplate.Infrastructure.Data
{
    public class DataContext(
        DbContextOptions<DataContext> options,
        IOptions<AuthorizationOptions> authorizationOptions)
        : DbContext(options)
    {

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authorizationOptions.Value));
        }
    }
}