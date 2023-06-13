namespace PedaloWebApp.Infrastructure.Data
{
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using PedaloWebApp.Core.Domain;
    using PedaloWebApp.Core.Interfaces.Data;

    public class DbContextFactory : IDbContextFactory
    {
        private static readonly Assembly ConfigurationsAssembly = typeof(DbContextFactory).Assembly;
        private readonly DbContextOptions<PedaloContext> options;

        public DbContextFactory(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<PedaloContext>();
            optionsBuilder.UseSqlServer(connectionString);

            this.options = optionsBuilder.Options;
        }

        /// <inheritdoc/>
        public PedaloContext CreateContext()
        {
            return new PedaloContext(options, ConfigurationsAssembly);
        }

        /// <inheritdoc/>
        public PedaloContext CreateReadOnlyContext()
        {
            var context = new PedaloContext(options, ConfigurationsAssembly);
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context;
        }
    }
}
