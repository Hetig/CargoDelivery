using System.Windows;
using System.Windows.Controls;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Views;

public partial class CreateCourier : Window
{
    public CreateCourierViewModel ViewModel;
    public CreateCourier(IApiService apiService)
    {
        ViewModel = new CreateCourierViewModel(apiService, this);
        DataContext = ViewModel;
        InitializeComponent();
    }
}