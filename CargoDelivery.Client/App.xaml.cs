using System.Net.Http;
using System.Windows;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CargoDelivery.Client;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<HttpClient>();
        services.AddSingleton<IApiService, ApiService>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainViewModel>();
    }
}