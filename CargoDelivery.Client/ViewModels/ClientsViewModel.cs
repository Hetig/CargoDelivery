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
        set
        {
            _clients = value;
            OnPropertyChanged(() => Clients);
        }
    }
    public ICommand OrdersCommand { get; }
    public ICommand ClientsCommand { get; }
    public ICommand CouriersCommand { get; }
    public ICommand CreateClientCommand { get; }
    public ICommand RefreshCommand { get; }
    
    public ClientsViewModel(IApiService apiService)
    {
        _apiService = apiService;
        
        CreateClientCommand = new RelayCommand(() => ScreenNavigator.GoToCreateClient());
        ClientsCommand = new RelayCommand(() => ScreenNavigator.GoToClients());
        OrdersCommand = new RelayCommand(() => ScreenNavigator.GoToOrders());
        CouriersCommand = new RelayCommand(() => ScreenNavigator.GoToCouriers());
        RefreshCommand = new RelayCommand(async () => await RefreshClients(), null);
        
        LoadClients().ConfigureAwait(false);
    }
    
    private async Task LoadClients()
    {
        var clients = await _apiService.GetClientsAsync();
        Clients = new ObservableCollection<Models.Client>(clients);
    }

    private async Task RefreshClients()
    {
        var clients = await _apiService.GetClientsAsync();
        Clients.Clear();
        foreach (var client in clients)
        {
            Clients.Add(client);
        }
    }
}