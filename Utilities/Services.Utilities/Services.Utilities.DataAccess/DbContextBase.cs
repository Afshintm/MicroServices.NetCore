using Microsoft.EntityFrameworkCore;

namespace Services.Utilities.DataAccess
{

    public class DbContextBase<TContext> : DbContext , IDbContext where TContext : DbContext
    {
        static DbContextBase()
        {
        }

        public DbContextBase(DbContextOptions<TContext> options) : base(options){ }

        public new DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
    }
}
