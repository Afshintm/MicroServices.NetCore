using System;
using System.Data.Entity;
using Common;

namespace DataAccess
{
	public static class DbContextBaseExtensions
	{
		public static void ApplyStateChanges<TContext>(this DbContextBase<TContext> dbContext) where TContext : DbContext
		{
			foreach (var dbEntityEntry in dbContext.ChangeTracker.Entries())
			{
				var entityState = dbEntityEntry.Entity as IObjectState;
				if (entityState == null)
					throw new InvalidCastException(
						"All entites must implement " +
						"the IObjectState interface, this interface " +
						"must be implemented so each entites state" +
						"can explicitely determined when updating graphs.");

				dbEntityEntry.State = ConvertState(entityState.ObjectState);
			}
		}

		private static EntityState ConvertState(ObjectState objectState)
		{
			switch (objectState)
			{
				case ObjectState.Added:
					return EntityState.Added;
				case ObjectState.Modified:
					return EntityState.Modified;
				case ObjectState.Deleted:
					return EntityState.Deleted;
				default:
					return EntityState.Unchanged;
			}
		}
	}        
}
