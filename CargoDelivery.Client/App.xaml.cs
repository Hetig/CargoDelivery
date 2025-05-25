using System.Net.Http;
using System.Windows;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;
using CargoDelivery.Client.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CargoDelivery.Client;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<MainViewModel>();
        services.AddTransient<OrdersViewModel>();

        services.AddSingleton<HttpClient>();
        services.AddSingleton<IApiService, ApiService>();
        services.AddSingleton<INavigationService>(provider =>
            new NavigationService(type => (ViewModelBase)provider.GetRequiredService(type)));

        services.AddSingleton<MainWindow>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }
}