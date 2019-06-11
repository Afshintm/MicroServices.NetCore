
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Services.Utilities.DataAccess
{
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        EntityEntry Entry(object entry);
        void Dispose();
        int SaveChanges();
    }
}
