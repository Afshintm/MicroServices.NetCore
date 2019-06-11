using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DataAccess
{
	public interface IDbContext
	{
		IDbSet<T> Set<T>() where T : class;
		DbEntityEntry Entry(object entry);
		void Dispose();
		int SaveChanges();
		Database Database { get; }
	}
}
