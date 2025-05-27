using System.Windows;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Views;

public partial class AssignToCourier : Window
{
    public Guid OrderId { get; }
    public AssignToCourierViewModel ViewModel;
    public AssignToCourier(IApiService apiService, Guid orderId)
    {
        OrderId = orderId;
        ViewModel = new AssignToCourierViewModel(apiService, this);
        DataContext = ViewModel;
        InitializeComponent();
    }
}