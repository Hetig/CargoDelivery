using System.Windows.Controls;
using CargoDelivery.Client.Services;

namespace CargoDelivery.Client.Views;

public partial class CreateCourier : Page
{
    public CreateCourier(IApiService apiService)
    {
        InitializeComponent();
    }
}