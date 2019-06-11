using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	public class DbContextBase<TContext> : DbContext, IDbContext where TContext : DbContext
	{
		static DbContextBase()
		{
			//Database.SetInitializer<TContext>(null);
		}


		public DbContextBase(string nameOrConnectionString) :
			base(nameOrConnectionString)
		{
			Configuration.LazyLoadingEnabled = false;
		}

		public new IDbSet<T> Set<T>() where T : class
		{
			return base.Set<T>();
		}
	}
}
