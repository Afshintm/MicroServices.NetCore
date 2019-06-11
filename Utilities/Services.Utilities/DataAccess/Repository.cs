using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Common;

namespace DataAccess
{
	public interface IRepository<TEntity> : IDisposable where TEntity : class
	{
		TEntity FindById(object id);
		void InsertGraph(TEntity entity);
		void Update(TEntity entity);
		void Delete(object id);
		void Delete(TEntity entity);
		void Insert(TEntity entity);
		RepositoryQuery<TEntity> Query();

	}

	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		internal IDbContext Context;
		internal IDbSet<TEntity> DbSet;

		public Repository(IDbContext context)
		{
			Context = context;
			DbSet = context.Set<TEntity>();
		}

		public virtual TEntity FindById(object id)
		{
			return DbSet.Find(id);
		}

		/// <summary>
		/// This method will make EF change track of all the entities in the graph in and marke them as added.
		/// This method should be called in conjunction with
		/// </summary>
		/// <param name="entity"></param>
		public virtual void InsertGraph(TEntity entity)
		{
			DbSet.Add(entity);
		}

		public virtual void Insert(TEntity entity)
		{
			// Adding this code ensures if developer called Commit() method without setting the ObjectState 
			// Adding of the root entity is still happering 
			var objectState = entity as IObjectState;
			if (objectState != null)
				objectState.ObjectState = ObjectState.Added;

			DbSet.Add(entity);
		}

		/// <summary>
		/// Update just the root entity unless 
		/// </summary>
		/// <param name="entity"></param>
		public virtual void Update(TEntity entity)
		{
			DbEntityEntry dbEntityEntry = Context.Entry(entity);
			dbEntityEntry.State = EntityState.Modified;

			// Adding this code ensures if developer called Commit() method without setting the ObjectState 
			// updating of the root entity is still happering 
			var objectState = entity as IObjectState;
			if (objectState != null)
				objectState.ObjectState = ObjectState.Modified;
		}

		public virtual void Delete(object id)
		{
			var entity = DbSet.Find(id);

			if (entity != null)
				Delete(entity);
		}

		public virtual void Delete(TEntity entity)
		{
			// Calling Remove on the entities which have not been tracked by EF causes InvalidOprationException so 
			// to make sure we do onot face the exception first we track the entity using Attach method then just remove it 
			// DbSet.Attach(entity);
			// DbSet.Remove(entity);

			// Or alternatively just set the state of the entity to Deleted

			DbEntityEntry dbEntityEntry = Context.Entry(entity);
			dbEntityEntry.State = EntityState.Deleted;

			var objectState = entity as IObjectState;
			if (objectState != null)
				objectState.ObjectState = ObjectState.Deleted;
		}

		public virtual RepositoryQuery<TEntity> Query()
		{
			var repositoryGetFluentHelper =
				new RepositoryQuery<TEntity>(this);

			return repositoryGetFluentHelper;
		}

		/// <summary>
		/// Repository Get method is accessable inside the repository assembly
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="orderBy"></param>
		/// <param name="includeProperties"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		internal IQueryable<TEntity> Get(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			List<Expression<Func<TEntity, object>>> includeProperties = null,
			int? page = null,
			int? pageSize = null)
		{
			IQueryable<TEntity> query = DbSet;

			if (includeProperties != null)
				includeProperties.ForEach(i => { query = query.Include(i); });

			if (filter != null)
				query = query.Where(filter);

			if (orderBy != null)
				query = orderBy(query);

			if (page != null && pageSize != null)
				query = query
					.Skip((page.Value - 1) * pageSize.Value)
					.Take(pageSize.Value);

			return query;
		}

		#region New accessor methods on Repository

		public void InsertGraphRoot(TEntity entity)
		{
			DbSet.Add(entity);
		}

		public TEntity Find(object id)
		{
			return DbSet.Find(id);
		}
		#endregion

		public void Dispose()
		{
			Context.Dispose();
		}
	}
}