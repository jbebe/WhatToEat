namespace WhatToEat.App.Services
{
	public abstract class AsyncServiceBase: IDisposable
	{
        private CancellationTokenSource CancellationTokenSource { get; set; }

        protected CancellationToken CancellationToken => CancellationTokenSource.Token;

        public AsyncServiceBase(CancellationTokenSource cancellationTokenSource)
        {
            CancellationTokenSource = cancellationTokenSource;
        }

        public abstract void Dispose();
	}
}
