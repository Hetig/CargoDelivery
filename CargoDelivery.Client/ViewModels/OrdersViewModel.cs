using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Models;
using CargoDelivery.Client.Services;
using CargoDelivery.Storage.Enums;

namespace CargoDelivery.Client.ViewModels;

public class OrdersViewModel : ViewModelBase
{
    private readonly IApiService _apiService;
    private ObservableCollection<Order> _orders;
    private Order _selectedOrder;
    private string _searchText;
    private bool _isBusy;

    public ObservableCollection<Order> Orders { get => _orders; set { _orders = value; OnPropertyChanged(); } }
    public Order SelectedOrder { get => _selectedOrder; set { _selectedOrder = value; OnPropertyChanged(); } }
    public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }

    public ICommand LoadOrdersCommand { get; }
    public ICommand AddOrderCommand { get; }
    public ICommand EditOrderCommand { get; }
    public ICommand DeleteOrderCommand { get; }
    public ICommand AssignToCourierCommand { get; }
    public ICommand CompleteOrderCommand { get; }
    public ICommand CancelOrderCommand { get; }

    public OrdersViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Orders = new ObservableCollection<Order>();

        LoadOrdersCommand = new RelayCommand(async () => await LoadOrders());
        DeleteOrderCommand = new RelayCommand(async () => await DeleteOrder());
        AssignToCourierCommand = new RelayCommand(async () => await AssignToCourier());
        CompleteOrderCommand = new RelayCommand(async () => await CompleteOrder());
        CancelOrderCommand = new RelayCommand(async () => await CancelOrder());

        LoadOrdersCommand.Execute(null);
    }

    private async Task LoadOrders()
    {
        IsBusy = true;
        try
        {
            var orders = await _apiService.GetOrdersAsync();
            Orders = new ObservableCollection<Order>(orders);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task DeleteOrder()
    {
        if (SelectedOrder == null) return;
        if (MessageBox.Show("Удалить заявку?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            await _apiService.DeleteOrderAsync(SelectedOrder.Id);
            Orders.Remove(SelectedOrder);
        }
    }

    private async Task AssignToCourier()
    {
        if (SelectedOrder == null) return;
        SelectedOrder.Status = OrderStatus.InProcess;
        await _apiService.UpdateOrderAsync(SelectedOrder);
    }

    private async Task CompleteOrder()
    {
        if (SelectedOrder == null) return;
        SelectedOrder.Status = OrderStatus.Done;
        await _apiService.OrderDoneAsync(SelectedOrder);
    }

    private async Task CancelOrder()
    {
        if (SelectedOrder == null) return;
        SelectedOrder.Status = OrderStatus.Cancelled;
        await _apiService.OrderCancelAsync(SelectedOrder);
    }
}