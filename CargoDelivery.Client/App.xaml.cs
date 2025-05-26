using System.Diagnostics;
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
    public static MainWindow MainWindow { get; private set; }
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
    
        var services = new ServiceCollection();
        ConfigureServices(services); 
    
        ServiceProvider = services.BuildServiceProvider(); 
    
        var apiService = ServiceProvider.GetRequiredService<IApiService>();
    
        Screens.CreateOrder = new CreateOrder(apiService);
        Screens.CreateClient = new CreateClient(apiService);
        Screens.CreateCourier = new CreateCourier(apiService);
        Screens.Orders = new Orders(apiService);
        Screens.EditOrder = new EditOrder(apiService);
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