using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Models;
using CargoDelivery.Client.Models.Queries;
using CargoDelivery.Client.Services;

namespace CargoDelivery.Client.ViewModels;

public class AssignToCourierViewModel : ViewModelBase
{
    public IApiService _apiService;
    public ICommand AssignCommand { get; }
    private Views.AssignToCourier View { get; }

    public AssignToCourierViewModel(IApiService apiService, Views.AssignToCourier view)
    {
        _apiService = apiService;
        
        View = view;
        AssignCommand = new RelayCommand(async () => await Assign(), CanAssign);
        LoadCouriers().ConfigureAwait(false);
    }
    
    public ObservableCollection<Models.Courier> Couriers { get; } = new();
    
    private async Task LoadCouriers()
    {
        var couriers = await _apiService.GetCouriersAsync();
        Couriers.Clear();
        foreach (var client in couriers)
        {
            Couriers.Add(client);
        }
    }
    
    private async Task Assign()
    {
        var courierId = ((Courier)View.Courier.SelectedItem).Id;
        var assigned = await _apiService.AssignToCourierAsync(View.OrderId, courierId);
        if (!assigned) MessageBox.Show("Не удалось назначить курьера");
        View.Close();
    }

    private bool CanAssign()
    {
        if (View.Courier.SelectedItem == null) return false;
        return true;
    }
}