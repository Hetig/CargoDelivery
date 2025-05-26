using System.Collections.ObjectModel;
using CargoDelivery.Client.Services;

namespace CargoDelivery.Client.ViewModels;

public class OrderCreateViewModel : ViewModelBase
{
    public IApiService _apiService;
    
    public OrderCreateViewModel(IApiService apiService)
    {
        _apiService = apiService;
        LoadClients().ConfigureAwait(false);
    }
    
    private ObservableCollection<Models.Client> _clients;
    
    public ObservableCollection<Models.Client> Clients
    {
        get => _clients;
        set => SetField(ref _clients, value);
    }
    
    private async Task LoadClients()
    {
        var clients = await _apiService.GetClientsAsync();
        Clients = new ObservableCollection<Models.Client>(clients);
    }
}