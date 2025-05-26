using System.Windows.Controls;
using CargoDelivery.Client.Services;

namespace CargoDelivery.Client.Views;

public partial class CreateClient : Page
{
    public CreateClient(IApiService apiService)
    {
        InitializeComponent();
    }
}