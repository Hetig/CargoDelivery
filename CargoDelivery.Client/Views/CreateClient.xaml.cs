using System.Windows;
using System.Windows.Controls;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Views;

public partial class CreateClient : Window
{
    public CreateClientViewModel ViewModel;
    public CreateClient(IApiService apiService)
    {
        ViewModel = new CreateClientViewModel(apiService, this);
        DataContext = ViewModel;
        InitializeComponent();
    }
}