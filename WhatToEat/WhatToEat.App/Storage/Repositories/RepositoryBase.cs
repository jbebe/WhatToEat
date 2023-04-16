namespace WhatToEat.App.Storage.Repositories
{
	public class RepositoryBase<T> where T: class
	{
		protected StorageContext Context { get; }

		public RepositoryBase(StorageContext context)
		{
			Context = context;
		}

		protected async Task AddAsync(T entity, CancellationToken cancellationToken)
		{
			await Context.GetDbSet<T>().AddAsync(entity, cancellationToken);
			await Context.SaveChangesAsync(cancellationToken);
		}
	}
}
