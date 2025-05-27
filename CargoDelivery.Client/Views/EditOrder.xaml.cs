using System.Windows;
using System.Windows.Controls;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Views;

public partial class EditOrder : Window
{
    public readonly Guid OrderId;
    public OrderEditViewModel ViewModel;
    public EditOrder(IApiService apiService, Guid orderId)
    {
        OrderId = orderId;
        ViewModel = new OrderEditViewModel(apiService, this);
        DataContext = ViewModel;
        InitializeComponent();
    }
}