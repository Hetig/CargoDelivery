using CargoDelivery.Client.Models;
using CargoDelivery.Client.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CargoDelivery.Client.Services;

public static class ScreenNavigator
{
    public static void GoToOrders()
    {
        App.MainWindow.Content = Screens.Orders;
    }
    public static void GoToClients()
    {
        App.MainWindow.Content = Screens.Clients;
    }
    public static void GoToCouriers()
    {
        App.MainWindow.Content = Screens.Couriers;
    }

    public static void GoToCreateOrder()
    {
        var apiService = App.ServiceProvider.GetRequiredService<IApiService>();
        var view = new CreateOrder(apiService);
        if (view.ShowDialog() == true)
        {
            GoToOrders();
        }
    }

    public static void GoToCreateCourier()
    {
        var apiService = App.ServiceProvider.GetRequiredService<IApiService>();
        var view = new CreateCourier(apiService);
        if (view.ShowDialog() == true)
        {
            GoToCouriers();
        }
    }
    public static void GoToCreateClient()
    {
        var apiService = App.ServiceProvider.GetRequiredService<IApiService>();
        var view = new CreateClient(apiService);
        view.ShowDialog();
    }

    public static void GoToEditOrder(Guid orderId)
    {
        var apiService = App.ServiceProvider.GetRequiredService<IApiService>();
        var view = new EditOrder(apiService, orderId);
        view.ShowDialog();
    }

    public static void AssignToCourier(Guid orderId)
    {
        var apiService = App.ServiceProvider.GetRequiredService<IApiService>();
        var view = new AssignToCourier(apiService, orderId);
        view.ShowDialog();
    }
}