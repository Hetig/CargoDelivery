using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Models;
using CargoDelivery.Client.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CargoDelivery.Client.Enums;

namespace CargoDelivery.Client.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        
        private List<Order> _orders;
        private Order _selectedOrder;
        private string _searchText;

        public List<Order> Orders
        {
            get => _orders;
            set => SetField(ref _orders, value);
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => SetField(ref _selectedOrder, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetField(ref _searchText, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand AssignCourierCommand { get; }
        public ICommand SetInProcessCommand { get; }
        public ICommand SetDoneCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }

        public OrdersViewModel(IApiService apiService, INavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            
            RefreshCommand = new RelayCommand(async () => await LoadOrders());
            SearchCommand = new RelayCommand(async () => await SearchOrders());
            EditCommand = new RelayCommand(EditOrder, CanEditOrder);
            AssignCourierCommand = new RelayCommand(AssignCourier, CanModifyOrder);
            SetInProcessCommand = new RelayCommand(async () => await SetOrderStatus(OrderStatus.InProcess), CanModifyOrder);
            SetDoneCommand = new RelayCommand(async () => await SetOrderStatus(OrderStatus.Done), CanModifyOrder);
            CancelCommand = new RelayCommand(async () => await CancelOrder(), CanModifyOrder);
            DeleteCommand = new RelayCommand(async () => await DeleteOrder(), CanModifyOrder);
            
            LoadOrders().ConfigureAwait(false);
        }

        private async Task LoadOrders()
        {
            var response = await _apiService.GetOrdersAsync();
            Orders = response?.Data ?? new List<Order>();
        }

        private async Task SearchOrders()
        {
            await LoadOrders();
            if (!string.IsNullOrEmpty(SearchText))
            {
                var searchTextLower = SearchText.ToLower();
                Orders = Orders.Where(o =>
                    o.Id.ToString().Contains(searchTextLower) ||
                    o.Status.ToString().ToLower().Contains(searchTextLower) ||
                    o.Client.Name.ToLower().Contains(searchTextLower) ||
                    o.TakeAddress.ToLower().Contains(searchTextLower) ||
                    o.DestinationAddress.ToLower().Contains(searchTextLower)
                ).ToList();
            }
        }

        private void EditOrder()
        {
            if (SelectedOrder != null && SelectedOrder.Status == OrderStatus.New)
            {
                _navigationService.NavigateTo(new OrderEditViewModel(_apiService, _navigationService, SelectedOrder));
            }
        }

        private async Task SetOrderStatus(OrderStatus status)
        {
            if (SelectedOrder != null)
            {
                var success = await _apiService.UpdateOrderStatusAsync(SelectedOrder.Id, status);
                if (success)
                {
                    await LoadOrders();
                }
            }
        }

        private async Task CancelOrder()
        {
            if (SelectedOrder != null)
            {
                var comment = "User cancelled"; // Get from dialog
                var success = await _apiService.UpdateOrderStatusAsync(SelectedOrder.Id, OrderStatus.Cancelled, comment);
                if (success)
                {
                    await LoadOrders();
                }
            }
        }

        private async Task DeleteOrder()
        {
            if (SelectedOrder != null)
            {
                var success = await _apiService.DeleteOrderAsync(SelectedOrder.Id);
                if (success)
                {
                    await LoadOrders();
                }
            }
        }

        private void AssignCourier()
        {
            
        }

        private bool CanEditOrder() => SelectedOrder != null && SelectedOrder.Status == OrderStatus.New;
        private bool CanModifyOrder() => SelectedOrder != null;
    }
}