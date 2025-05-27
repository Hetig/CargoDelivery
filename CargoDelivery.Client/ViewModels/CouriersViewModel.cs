using System.Collections.ObjectModel;
using System.Windows.Input;
using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Models;
using CargoDelivery.Client.Services;

namespace CargoDelivery.Client.ViewModels;

public class CouriersViewModel : ViewModelBase
{
    private readonly IApiService _apiService;
        
    private ObservableCollection<Courier> _couriers;

    public ObservableCollection<Courier> Couriers
    {
        get => _couriers;
        set
        {
            _couriers = value;
            OnPropertyChanged(() => Couriers);
        }
    }
    public ICommand OrdersCommand { get; }
    public ICommand ClientsCommand { get; }
    public ICommand CouriersCommand { get; }
    public ICommand CreateCourierCommand { get; }
    public ICommand RefreshCommand { get; }

    public CouriersViewModel(IApiService apiService)
    {
        _apiService = apiService;
        
        CreateCourierCommand = new RelayCommand(() => ScreenNavigator.GoToCreateCourier());
        ClientsCommand = new RelayCommand(() => ScreenNavigator.GoToClients());
        OrdersCommand = new RelayCommand(() => ScreenNavigator.GoToOrders());
        CouriersCommand = new RelayCommand(() => ScreenNavigator.GoToCouriers());
        RefreshCommand = new RelayCommand(async () => await RefreshCourier(), null);
        
        LoadCouriers().ConfigureAwait(false);
    }
    
    private async Task LoadCouriers()
    {
        var couriers = await _apiService.GetCouriersAsync();
        Couriers = new ObservableCollection<Courier>(couriers);
    }

    private async Task RefreshCourier()
    {
        var couriers = await _apiService.GetCouriersAsync();
        Couriers.Clear();
        foreach (var courier in couriers)
        {
            Couriers.Add(courier);
        }
    }
}