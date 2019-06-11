using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;

namespace Services.Utilities.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        void Dispose(bool disposing);
        IRepository<T> Repository<T>() where T : class;
    }

    /// <summary>
    /// Unit of Work around a context
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : IDbContext
    {
        TContext Context { get; set; }
    }

    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : IDbContext
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

        //public UnitOfWork() : this(new TContext()) { }

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
        

        //create or load generic Repository that works based on unitOfWork context
        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var genericRepoType = repositoryType.MakeGenericType(typeof(T));
                var repositoryInstance =
                    Activator.CreateInstance(genericRepoType, _context);

                //save repository in memory
                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }

    }

}