using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Common;

namespace DataAccess
{

	 public interface IBoundedDbContext : IDbContext
    {
        event ObjectMaterializedEventHandler ObjectMaterialized;
    }

	public abstract class BoundDbContext<TContext> : DbContextBase<TContext>, IBoundedDbContext where TContext : DbContext
	{
		public BoundDbContext(string nameOrConnectionString) :
			base(nameOrConnectionString)
		{
			((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += OnObjectMaterialized;
		}

		public BoundDbContext() : this("Sales") { }

		void OnObjectMaterialized(object sender, ObjectMaterializedEventArgs args)
		{
			var entity = args.Entity as IObjectState;
			if (entity != null)
			{
				entity.ObjectState = ObjectState.Unchanged;
			}

			if (ObjectMaterialized != null)
			{
				ObjectMaterialized(sender, args);
			}
		}

		public event ObjectMaterializedEventHandler ObjectMaterialized;

		public override int SaveChanges()
		{
			this.ApplyStateChanges<TContext>();
			return base.SaveChanges();
		}

		public int Save()
		{
			return base.SaveChanges();
		}
	}

}
