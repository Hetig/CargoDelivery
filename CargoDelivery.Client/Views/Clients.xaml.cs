using System.Windows.Controls;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Views;

public partial class Clients : Page
{
    public ClientsViewModel ViewModel;
    public Clients(IApiService apiService)
    {
        ViewModel = new ClientsViewModel(apiService);
        DataContext = ViewModel;
        InitializeComponent();
    }
}