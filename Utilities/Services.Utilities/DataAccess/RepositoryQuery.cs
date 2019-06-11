using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace DataAccess
{
	public sealed class RepositoryQuery<TEntity> where TEntity : class
	{
		private readonly Repository<TEntity> _repository;
		private readonly List<Expression<Func<TEntity, object>>> _includeProperties;
		private Expression<Func<TEntity, bool>> _filter;
		private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> _orderByQuerable;
		private int? _page;
		private int? _pageSize;

		public RepositoryQuery(Repository<TEntity> repository)
		{
			_repository = repository;
			_includeProperties = new List<Expression<Func<TEntity, object>>>();
		}

		public RepositoryQuery<TEntity> Filter(Expression<Func<TEntity, bool>> filter)
		{
			_filter = filter;
			return this;
		}

		public RepositoryQuery<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
		{
			_orderByQuerable = orderBy;
			return this;
		}

		public RepositoryQuery<TEntity> Include(Expression<Func<TEntity, object>> expression)
		{
			_includeProperties.Add(expression);
			return this;
		}

		/// <summary>
		/// Include all the association relationship passed
		/// </summary>
		/// <param name="includeProperties">a list of all the expression to be included</param>
		/// <returns></returns>
		public RepositoryQuery<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			foreach (var includeProperty in includeProperties)
			{
				_includeProperties.Add(includeProperty);
			}
			return this;
		}


		public IEnumerable<TEntity> GetPage(int page, int pageSize, out int totalCount)
		{
			_page = page;
			_pageSize = pageSize;
			totalCount = _repository.Get(_filter).Count();

			return _repository.Get(
				_filter,
				_orderByQuerable, _includeProperties, _page, _pageSize);
		}

		public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null)
		{
			// changing the filter of Get if specified
			if (filter != null)
			{
				_filter = filter;
			}
			else
			{
				//this method is woking as long as a filter has been specified otherwise Get() method shoud be called to return IQueryable<TEntity>  
				if (_filter == null && _orderByQuerable == null && _includeProperties.Count == 0)
					throw new InvalidOperationException("The Query needs to be filtered you can use Get() method. ");
			}
			return _repository.Get(
				_filter,
				_orderByQuerable, _includeProperties, _page, _pageSize);
		}

		public IQueryable<TEntity> Get()
		{

			return _repository.Get(
			_filter,
			_orderByQuerable, _includeProperties, _page, _pageSize);

		}
	}
}