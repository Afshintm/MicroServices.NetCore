using System;
using System.Collections;

namespace DataAccess
{
	public interface IUnitOfWork : IDisposable
	{
		void Save();
		void Dispose(bool disposing);
		IRepository<T> Repository<T>() where T : class;
	}

	public class BaseUnitOfWork : IUnitOfWork
	{
		private bool _disposed;
		private Hashtable _repositories;
		private IDbContext _context;

		public BaseUnitOfWork(IDbContext dbContext)
		{
			_context = dbContext;
		}

		public virtual void Save()
		{
		}

		public virtual void Dispose(bool disposing)
		{
			if (!_disposed)
				if (disposing)
				{
					//_repositories = null;
					_context.Dispose();
				}

			_disposed = true;

		}

		public virtual IRepository<T> Repository<T>() where T : class
		{
			if (_repositories == null)
				_repositories = new Hashtable();

			var type = typeof(T).Name;

			if (!_repositories.ContainsKey(type))
			{
				var repositoryType = typeof(Repository<>);

				var repositoryInstance =
					Activator.CreateInstance(repositoryType
							.MakeGenericType(typeof(T)), _context);

				_repositories.Add(type, repositoryInstance);
			}

			return (IRepository<T>)_repositories[type];

		}

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

	/// <summary>
	/// Unit of Work around a context
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : IDbContext
	{
		TContext Context { get; set;}
		
	}

	public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : IDbContext, new()
	{
		private bool _disposed;
		private Hashtable _repositories;
		private TContext _context;
		public TContext Context
		{
			get
			{
				return _context;
			}
			set
			{
				_context = value;
			}
		}

		public UnitOfWork() : this(new TContext()) { }

		public UnitOfWork(TContext context)
		{
			_context = context;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Save()
		{
			_context.SaveChanges();
		}

		public virtual void Dispose(bool disposing)
		{
			if (!_disposed)
				if (disposing)
					_context.Dispose();

			_disposed = true;
		}

		public IRepository<T> Repository<T>() where T : class
		{
			if (_repositories == null)
				_repositories = new Hashtable();

			var type = typeof(T).Name;

			if (!_repositories.ContainsKey(type))
			{
				var repositoryType = typeof(Repository<>);

				var repositoryInstance =
					Activator.CreateInstance(repositoryType
							.MakeGenericType(typeof(T)), _context);

				_repositories.Add(type, repositoryInstance);
			}

			return (IRepository<T>)_repositories[type];
		}

	}


	public interface IBoundedUnitOfWork
	{
		int Commit();
	}

	public interface IBoundedUnitOfWork<TContext> : IBoundedUnitOfWork, IUnitOfWork<TContext> where TContext : IBoundedDbContext { }




	
}