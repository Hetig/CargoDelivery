using System.Windows.Controls;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Views;

public partial class Orders : Page
{
    public OrdersViewModel ViewModel;
    public Orders(IApiService apiService)
    {
        ViewModel = new OrdersViewModel(apiService);
        DataContext = ViewModel;
        InitializeComponent();
    }
}