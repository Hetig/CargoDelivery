using System.Windows;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Views;

public partial class CreateOrder : Window
{
    public OrderCreateViewModel ViewModel;
    public CreateOrder(IApiService apiService)
    {
        ViewModel = new OrderCreateViewModel(apiService, this);
        DataContext = ViewModel;
        InitializeComponent();
    }
}