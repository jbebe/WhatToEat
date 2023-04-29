using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace WhatToEat.App.Storage.Repositories
{
    public class RepositoryBase<T> where T : class
	{
		protected StorageContext Context { get; }

		private Lazy<DbSet<T>> DbSet { get; }

		public RepositoryBase(StorageContext context)
		{
			Context = context;
			DbSet = new Lazy<DbSet<T>>(() => Context.GetDbSet<T>());
		}

		protected async Task CreateOrUpdateAsync(T entity, Expression<Func<T, bool>> compare, Action<T>? update, CancellationToken? cancellationToken = null)
		{
			cancellationToken ??= CancellationToken.None;
			var foundEntity = await DbSet.Value.FirstOrDefaultAsync(compare, cancellationToken.Value);
			if (foundEntity != null)
			{
				update?.Invoke(foundEntity);
				DbSet.Value.Update(foundEntity);
			}
			else
			{
				DbSet.Value.Add(entity);
			}

			await Context.SaveChangesAsync(cancellationToken.Value);
		}

        public async Task<List<T>> GetAllAsync(
            Func<DbSet<T>, IQueryable<T>>? addIncludes = null,
            CancellationToken? cancellationToken = null) =>
			await QueryAsync(whereExpression: null, addIncludes, cancellationToken);

        public async Task<List<T>> QueryAsync(
			Expression<Func<T, bool>>? whereExpression = null,
			Func<DbSet<T>, IQueryable<T>>? addIncludes = null,
			CancellationToken? cancellationToken = null)
		{
			var query = addIncludes != null
				? addIncludes(DbSet.Value)
				: DbSet.Value;
			query = whereExpression != null
				? query.Where(whereExpression)
				: query;
			return await query.ToListAsync(cancellationToken ?? CancellationToken.None);
		}

        public async Task<T?> GetAsync(
            Expression<Func<T, bool>> whereExpression,
            Func<DbSet<T>, IQueryable<T>>? addIncludes = null,
            CancellationToken? cancellationToken = null)
        {
            var query = addIncludes != null
                ? addIncludes(DbSet.Value)
                : DbSet.Value;
			return await query.FirstOrDefaultAsync(whereExpression, cancellationToken ?? CancellationToken.None);
        }
    }
}
