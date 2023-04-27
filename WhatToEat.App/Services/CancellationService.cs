namespace WhatToEat.App.Services;

public class CancellationService: IDisposable
{
    private CancellationTokenSource TokenSource { get; }

    public CancellationToken CancellationToken { get; }

    public CancellationService()
    {
        TokenSource = new CancellationTokenSource();
        CancellationToken = TokenSource.Token;
    }

    public void Dispose()
    {
        TokenSource.Cancel();
        TokenSource.Dispose();
    }
}
