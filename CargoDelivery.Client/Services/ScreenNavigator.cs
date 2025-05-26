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
            var newOrder = new Order();            
            //OrderRepository.Add(newOrder);
            
            GoToOrders();
        }
    }

    public static void GoToCreateCourier()
    {
        App.MainWindow.Content = Screens.CreateCourier;
    }
    public static void GoToCreateClient()
    {
        App.MainWindow.Content = Screens.CreateClient;
    }
}