using System.Windows.Input;
using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Services;

namespace CargoDelivery.Client.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    
    public ICommand NavigateHomeCommand { get; }

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        
        NavigateHomeCommand = new RelayCommand(NavigateToHome);
        
        _navigationService.NavigateTo<OrdersViewModel>();
    }
    
    private void NavigateToHome() => _navigationService.NavigateTo<OrdersViewModel>();
}