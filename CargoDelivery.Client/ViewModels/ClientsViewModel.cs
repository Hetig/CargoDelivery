using System.Collections.ObjectModel;
using System.Windows.Input;
using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Models;
using CargoDelivery.Client.Services;

namespace CargoDelivery.Client.ViewModels;

public class ClientsViewModel : ViewModelBase
{
    private readonly IApiService _apiService;
        
    private ObservableCollection<Models.Client> _clients;
    
    public ObservableCollection<Models.Client> Clients
    {
        get => _clients;
        set => SetField(ref _clients, value);
    }
    public ICommand OrdersCommand { get; }
    public ICommand ClientsCommand { get; }
    public ICommand CouriersCommand { get; }
    public ICommand CreateClientCommand { get; }
    
    public ClientsViewModel(IApiService apiService)
    {
        _apiService = apiService;
        
        CreateClientCommand = new RelayCommand(() => ScreenNavigator.GoToCreateClient());
        ClientsCommand = new RelayCommand(() => ScreenNavigator.GoToClients());
        OrdersCommand = new RelayCommand(() => ScreenNavigator.GoToOrders());
        CouriersCommand = new RelayCommand(() => ScreenNavigator.GoToCouriers());
        
        LoadClients().ConfigureAwait(false);
    }
    
    private async Task LoadClients()
    {
        var clients = await _apiService.GetClientsAsync();
        Clients = new ObservableCollection<Models.Client>(clients);
    }
}