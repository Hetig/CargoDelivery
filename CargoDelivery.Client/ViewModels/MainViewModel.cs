using System.Windows.Input;
using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Services;
using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ICommand NavigateToOrdersCommand { get; }
        public ICommand NavigateToCreateOrderCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            
            NavigateToOrdersCommand = new RelayCommand(NavigateToOrders);
            NavigateToCreateOrderCommand = new RelayCommand(NavigateToCreateOrder);
            
            NavigateToOrders();
        }

        private void NavigateToOrders()
        {
            _navigationService.NavigateTo<OrdersViewModel>();
        }

        private void NavigateToCreateOrder()
        {
            _navigationService.NavigateTo<OrderCreateViewModel>();
        }
    }
}