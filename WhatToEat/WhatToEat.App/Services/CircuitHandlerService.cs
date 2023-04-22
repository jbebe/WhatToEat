using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.JSInterop;
using System.Collections.Concurrent;

namespace WhatToEat.App.Services;

public class CircuitHandlerService : CircuitHandler
{
    private readonly Guid _instanceId = Guid.NewGuid();
    private readonly ILogger<CircuitHandlerService> _logger;
    private readonly IJSRuntime _jsRuntime;
    private readonly CircuitStore _circuitStore;

    public CircuitHandlerService(ILogger<CircuitHandlerService> logger, IJSRuntime jsRuntime, CircuitStore circuitStore)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _circuitStore = circuitStore ?? throw new ArgumentNullException(nameof(circuitStore));

        _logger.LogInformation("Circuit Handler created with Id {CircuitHandlerId}", _instanceId);
    }

    public override async Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CircuitHandler {CircuitHandlerId} got {CircuitEvent} for Circuit {CircuitId}", _instanceId, "OPENED", circuit.Id);

        var remoteSessionId = await GetRemoteSessionIdAsync(cancellationToken);
        if (!String.IsNullOrEmpty(remoteSessionId))
        {
            _logger.LogInformation("CircuitHandler {CircuitHandlerId} FOUND remote session {RemoteSessionId} for Circuit {CircuitId}", _instanceId, remoteSessionId, circuit.Id);
        }
        else
        {
            remoteSessionId = Guid.NewGuid().ToString();
            await SetRemoteSessionIdAsync(remoteSessionId, cancellationToken);
            _logger.LogInformation("CircuitHandler {CircuitHandlerId} CREATED {RemoteSessionId} for Circuit {CircuitId}", _instanceId, remoteSessionId, circuit.Id);
        }

        _circuitStore.InitializeCircuitId(circuit.Id, remoteSessionId);
        await _circuitStore.SaveValueAsync($"LastAccessed", DateTime.UtcNow.ToString("g"), cancellationToken);

        await base.OnCircuitOpenedAsync(circuit, cancellationToken);
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CircuitHandler {CircuitHandlerId} got {CircuitEvent} for Circuit {CircuitId}", _instanceId, "connection down", circuit.Id);

        return base.OnConnectionDownAsync(circuit, cancellationToken);
    }

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CircuitHandler {CircuitHandlerId} got {CircuitEvent} for Circuit {CircuitId}", _instanceId, "connection up", circuit.Id);

        return base.OnConnectionUpAsync(circuit, cancellationToken);
    }

    public override async Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CircuitHandler {CircuitHandlerId} got {CircuitEvent} for Circuit {CircuitId}", _instanceId, "CLOSED", circuit.Id);

        await _circuitStore.SaveValueAsync($"LastAccessed", DateTime.UtcNow.ToString("g"), cancellationToken);
        await base.OnCircuitClosedAsync(circuit, cancellationToken);
    }

    #region JS Interop

    private const string RemoteSessionIdKey = "sessionId";

    private async Task<string> GetRemoteSessionIdAsync(CancellationToken cancellationToken = default)
    {
        return await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", cancellationToken, RemoteSessionIdKey);
    }

    private async Task SetRemoteSessionIdAsync(string value, CancellationToken cancellationToken = default)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", cancellationToken, RemoteSessionIdKey, value);
    }

    #endregion
}

public class CircuitStore
{
    private readonly Guid _instanceId = Guid.NewGuid();
    private readonly ILogger<CircuitStore> _logger;
    private readonly IServiceProvider _serviceProvider;

    private string _circuitId;
    private string _sessionId;

    public bool HasCircuit => !String.IsNullOrEmpty(_circuitId);

    public CircuitStore(ILogger<CircuitStore> logger, IServiceProvider serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public void InitializeCircuitId(string circuitId, string sessionId)
    {
        _logger.LogInformation("CircuitStore {StoreInstanceId} initialized with CircuitId {CircuitId}", _instanceId, circuitId);

        _circuitId = circuitId;
        _sessionId = sessionId;
    }

    public async Task<string> LoadValueAsync(string key, CancellationToken cancellationToken = default)
    {
        if (!HasCircuit)
            return null;

        using var scope = _serviceProvider.CreateScope();
        await using var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
        var entry = await ctx.Values.FirstOrDefaultAsync(v => v.Key == $"{_sessionId}_{key}", cancellationToken);

        _logger.LogInformation("CircuitStore {StoreInstanceId} found value {value} for key {key} on circuit {CircuitId}", _instanceId, entry?.Value, key, _circuitId);

        return entry?.Value;
    }

    public async Task SaveValueAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        if (!HasCircuit)
            return;

        using var scope = _serviceProvider.CreateScope();
        await using var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
        var entry = await ctx.Values.FirstOrDefaultAsync(v => v.Key == $"{_sessionId}_{key}", cancellationToken);

        if (entry == null)
        {
            entry = new KeyValue() { Key = $"{_sessionId}_{key}" };
            await ctx.Values.AddAsync(entry, cancellationToken);
        }

        entry.Value = value;

        _logger.LogInformation("CircuitStore {StoreInstanceId} saved value {value} for key {key} on circuit {CircuitId}", _instanceId, value, key, _circuitId);

        await ctx.SaveChangesAsync(cancellationToken);
    }
}