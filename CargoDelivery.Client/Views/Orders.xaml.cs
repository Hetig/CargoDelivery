using System.Windows.Controls;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Views;

public partial class Orders : UserControl
{
    public Orders(OrdersViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}