using System.Windows.Input;
using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Models;
using CargoDelivery.Client.Models.Queries;
using CargoDelivery.Client.Services;

namespace CargoDelivery.Client.ViewModels;

public class OrderEditViewModel : ViewModelBase
{
    public IApiService _apiService;
    public ICommand SaveCommand { get; }
    private Views.EditOrder View { get; }

    public OrderEditViewModel(IApiService apiService, Views.EditOrder view)
    {
        _apiService = apiService;
        
        View = view;

        SaveCommand = new RelayCommand(async () => await Save(), CanSave);
        LoadOrder().ConfigureAwait(false);
    }

    public async Task LoadOrder()
    {
        var order = await _apiService.GetOrderByIdAsync(View.OrderId);
        if (order == null) return;
        
        View.TakeAddress.Text = order.TakeAddress;
        View.DestinationAddress.Text = order.DestinationAddress;
        View.DestinationDateTime.Value = order.DestinationDateTime;
        View.TakeDateTime.Value = order.TakeDateTime;
    }
    
    private async Task Save()
    {
        var editOrder = new EditOrder()
        {
            Id = View.OrderId,
            TakeDateTime = ((DateTime)View.TakeDateTime.Value).ToUniversalTime(),
            TakeAddress = View.TakeAddress.Text,
            DestinationAddress = View.DestinationAddress.Text,
            DestinationDateTime = ((DateTime)View.DestinationDateTime.Value).ToUniversalTime(),
        };

        var changed = await _apiService.UpdateOrderAsync(editOrder);
        View.Close();
    }

    private bool CanSave()
    {
        if (string.IsNullOrWhiteSpace(View.TakeAddress.Text)) return false;
        if (string.IsNullOrWhiteSpace(View.DestinationAddress.Text)) return false;
        if (View.TakeDateTime?.Value == null) return false;
        if (View.DestinationDateTime?.Value == null) return false;
        if (View.TakeDateTime?.Value > View.DestinationDateTime?.Value) return false;
        return true;
    }
}