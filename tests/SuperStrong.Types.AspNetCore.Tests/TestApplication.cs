using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SuperStrong.Types.AspNetCore.Tests;

internal sealed class TestApplication : IAsyncDisposable
{
    private readonly WebApplication _app;

    public HttpClient Client { get; }

    private TestApplication(WebApplication app, HttpClient client)
    {
        _app = app;
        Client = client;
    }

    public static Task<TestApplication> StartAsync(Action<WebApplication> configure)
    {
        return StartAsync(builder => { }, configure);
    }

    public static Task<TestApplication> StartAsync<TController>()
        where TController : ControllerBase
    {
        return StartAsync<TController>(builder => { }, app => { });
    }

    public static Task<TestApplication> StartAsync<TController>(
        Action<WebApplicationBuilder> configureBuilder,
        Action<WebApplication> configure)
        where TController : ControllerBase
    {
        return StartAsync(
            builder =>
            {
                builder.Services
                    .AddControllers()
                    .ConfigureApplicationPartManager(manager =>
                    {
                        // replace the assembly-scanning provider with one that registers only TController
                        var controllerFeatureProvider = manager.FeatureProviders.OfType<ControllerFeatureProvider>().Single();
                        manager.FeatureProviders.Remove(controllerFeatureProvider);
                        manager.FeatureProviders.Add(new SingleControllerFeatureProvider(typeof(TController)));
                    });

                configureBuilder(builder);
            },
            app =>
            {
                app.MapControllers();
                configure(app);
            });
    }

    public static async Task<TestApplication> StartAsync(
        Action<WebApplicationBuilder> configureBuilder,
        Action<WebApplication> configure)
    {
        var builder = WebApplication.CreateSlimBuilder();

        builder.WebHost.UseTestServer();

        configureBuilder(builder);

        var app = builder.Build();
        configure(app);

        await app.StartAsync(TestContext.Current.CancellationToken);

        return new TestApplication(app, app.GetTestClient());
    }

    public async ValueTask DisposeAsync()
    {
        Client.Dispose();

        await _app.DisposeAsync();
    }

    private sealed class SingleControllerFeatureProvider(Type controllerType) : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            feature.Controllers.Add(controllerType.GetTypeInfo());
        }
    }
}
