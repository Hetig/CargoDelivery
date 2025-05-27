using System.Collections.ObjectModel;
using System.Windows.Input;
using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Models;
using CargoDelivery.Client.Models.Queries;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.Views;
using CreateOrder = CargoDelivery.Client.Models.Queries.CreateOrder;

namespace CargoDelivery.Client.ViewModels;

public class OrderCreateViewModel : ViewModelBase
{
    public IApiService _apiService;
    private CreateOrder _createOrder;
    public ICommand SaveCommand { get; }
    private Views.CreateOrder View { get; }

    public OrderCreateViewModel(IApiService apiService, Views.CreateOrder view)
    {
        _apiService = apiService;
        
        View = view;
        SaveCommand = new RelayCommand(async () => await Save(), CanSave);
        LoadClients().ConfigureAwait(false);
    }

    public ObservableCollection<Models.Client> Clients { get; } = new();

    private async Task LoadClients()
    {
        var clients = await _apiService.GetClientsAsync();
        Clients.Clear();
        foreach (var client in clients)
        {
            Clients.Add(client);
        }
    }

    private async Task Save()
    {
        var newOrder = new CreateOrder()
        {
            ClientId = ((Models.Client)View.Client.SelectedItem).Id,
            Cargo = new CreateCargo()
            {
                Name = View.CargoName.Text,
                Description = View.CargoDescription.Text,
            },
            TakeDateTime = ((DateTime)View.TakeDateTime.Value).ToUniversalTime(),
            TakeAddress = View.TakeAddress.Text,
            DestinationAddress = View.DestinationAddress.Text,
            DestinationDateTime = ((DateTime)View.DestinationDateTime.Value).ToUniversalTime(),
        };
        
        var createdOrder = await _apiService.CreateOrderAsync(newOrder);
        View.Close();
    }

    private bool CanSave()
    {
        if (View.Client.SelectedItem == null) return false;
        if (string.IsNullOrWhiteSpace(View.CargoName.Text)) return false;
        if (string.IsNullOrWhiteSpace(View.CargoDescription.Text)) return false;
        if (string.IsNullOrWhiteSpace(View.TakeAddress.Text)) return false;
        if (string.IsNullOrWhiteSpace(View.DestinationAddress.Text)) return false;
        if (View.TakeDateTime?.Value == null) return false;
        if (View.DestinationDateTime?.Value == null) return false;
        if (View.TakeDateTime?.Value > View.DestinationDateTime?.Value) return false;
        if (View.TakeDateTime?.Value  < DateTime.Now) return false;
        return true;
    }
}