using System.Windows.Controls;
using CargoDelivery.Client.Services;

namespace CargoDelivery.Client.Views;

public partial class EditOrder : Page
{
    public EditOrder(IApiService apiService)
    {
        InitializeComponent();
    }
}