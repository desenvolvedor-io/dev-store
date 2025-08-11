using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;

namespace DevStore.Tests;

public abstract class IntegrationTest<TProgram>(string baseUrl) :
    IClassFixture<WebApplicationFactory<TProgram>>,
    IAsyncLifetime,
    IDisposable
    where TProgram : class
{
    private readonly WebApplicationFactory<TProgram> _webApiFactory = new();

    protected HttpClient HttpClient = null!;

    public Task InitializeAsync()
    {
        HttpClient = _webApiFactory.CreateClient();
        HttpClient.BaseAddress = new Uri(baseUrl);
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _webApiFactory.Dispose();
        HttpClient.Dispose();
    }

    protected async Task ExecuteInScope<T>(Func<T, Task> action) where T : notnull
    {
        await using var scope = _webApiFactory.Services.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        await action(service);
    }

    protected async Task<TResult> ExecuteInScope<TService, TResult>(Func<TService, Task<TResult>> action)
        where TService : notnull
    {
        await using var scope = _webApiFactory.Services.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<TService>();
        return await action(service);
    }

    protected static async Task ShouldEventuallyAssert(Func<Task> assert, TimeSpan? timeout = null,
        TimeSpan? interval = null)
    {
        timeout ??= TimeSpan.FromSeconds(5);
        interval ??= TimeSpan.FromMilliseconds(150);
        var stopwatch = Stopwatch.StartNew();
        while (true)
        {
            try
            {
                await assert();
            }
            catch (XunitException)
            {
                if (stopwatch.Elapsed > timeout.Value)
                    throw;

                await Task.Delay(interval.Value);
                continue;
            }

            break;
        }
    }
}