using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Example.Data.Data
{
    public class RepositoryContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public RepositoryContext()
        {
        }

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        public RepositoryContext(DbContextOptions<RepositoryContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<AppUser> AppUsers { get; set; }
    }
}
