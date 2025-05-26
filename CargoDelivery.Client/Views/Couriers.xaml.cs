using System.Windows.Controls;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Views;

public partial class Couriers : Page
{
    public CouriersViewModel ViewModel { get; }
    public Couriers(IApiService apiService)
    {
        ViewModel = new CouriersViewModel(apiService);
        DataContext = ViewModel;
        InitializeComponent();
    }
}