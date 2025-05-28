using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;
using CargoDelivery.Client.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CargoDelivery.Client;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IConfiguration Configuration { get; private set; }
    public static IServiceProvider ServiceProvider { get; private set; }
    public static MainWindow MainWindow { get; private set; }
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
    
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();
        
        var services = new ServiceCollection();
        ConfigureServices(services); 
    
        ServiceProvider = services.BuildServiceProvider(); 
    
        var apiService = ServiceProvider.GetRequiredService<IApiService>();
        
        Screens.Orders = new Orders(apiService);
        Screens.Clients = new Clients(apiService);
        Screens.Couriers = new Couriers(apiService);
    
        MainWindow = new MainWindow();
        MainWindow.Show();
        ScreenNavigator.GoToOrders();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<HttpClient>();
        services.AddSingleton<IApiService, ApiService>();
    }
}