using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Models;
using CargoDelivery.Client.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CargoDelivery.Client.Enums;

namespace CargoDelivery.Client.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        
        private ObservableCollection<Order> _orders;
        private Order _selectedOrder;
        private string _searchText;

        public ObservableCollection<Order> Orders
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
        public ICommand CreateCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand AssignCourierCommand { get; }
        public ICommand SetInProcessCommand { get; }
        public ICommand SetDoneCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand OrdersCommand { get; }
        public ICommand ClientsCommand { get; }
        public ICommand CouriersCommand { get; }

        public OrdersViewModel(IApiService apiService)
        {
            _apiService = apiService;
            
            RefreshCommand = new RelayCommand(async () => await LoadOrders());
            SearchCommand = new RelayCommand(async () => await SearchOrders());
            EditCommand = new RelayCommand(EditOrder, CanEditOrder);
            CreateCommand = new RelayCommand(CreateOrder, null);
            AssignCourierCommand = new RelayCommand(AssignCourier, CanModifyOrder);
            SetInProcessCommand = new RelayCommand(async () => await InProcessOrder(), CanModifyOrder);
            SetDoneCommand = new RelayCommand(async () => await DoneOrder(), CanModifyOrder);
            CancelCommand = new RelayCommand(async () => await CancelOrder(), CanModifyOrder);
            DeleteCommand = new RelayCommand(async () => await DeleteOrder(), CanModifyOrder);

            OrdersCommand = new RelayCommand(() => ScreenNavigator.GoToOrders());
            ClientsCommand = new RelayCommand(() => ScreenNavigator.GoToClients());
            CouriersCommand = new RelayCommand(() => ScreenNavigator.GoToCouriers());
            
            LoadOrders().ConfigureAwait(false);
        }

        private async Task LoadOrders()
        {
            var response = await _apiService.GetOrdersAsync();
            var orders = response?.Data ?? new List<Order>();
            Orders = new ObservableCollection<Order>(orders);
        }

        private async Task SearchOrders()
        {
            await LoadOrders();
            if (!string.IsNullOrEmpty(SearchText))
            {
                var searchTextLower = SearchText.ToLower();
                var orders = Orders.Where(o =>
                    o.Id.ToString().Contains(searchTextLower) ||
                    o.Status.ToString().ToLower().Contains(searchTextLower) ||
                    o.Client.Name.ToLower().Contains(searchTextLower) ||
                    o.TakeAddress.ToLower().Contains(searchTextLower) ||
                    o.DestinationAddress.ToLower().Contains(searchTextLower)
                ).ToList();
                
                Orders = new ObservableCollection<Order>(orders);
            }
        }

        private void CreateOrder()
        {
            ScreenNavigator.GoToCreateOrder();
        }
        private void EditOrder()
        {
            if (SelectedOrder != null && SelectedOrder.Status == OrderStatus.New)
            {
                // Navigate to Edit Order
            }
        }

        private async Task DoneOrder()
        {
            if (SelectedOrder != null)
            {
                var success = await _apiService.SetDoneAsync(SelectedOrder.Id);
                if (success)
                {
                    await LoadOrders();
                }
            }
        }
        private async Task InProcessOrder()
        {
            if (SelectedOrder != null)
            {
                var success = await _apiService.SetInProcessAsync(SelectedOrder.Id);
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
                var comment = "User cancelled";
                var success = await _apiService.SetCancelAsync(SelectedOrder.Id, comment);
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