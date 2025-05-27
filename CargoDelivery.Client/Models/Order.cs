using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.CompilerServices;
using CargoDelivery.Client.Enums;

namespace CargoDelivery.Client.Models;

public class Order : INotifyPropertyChanged
{
    private Guid _id;
    private Client _client;
    private DateTime _createDateTime;
    private Cargo _cargo = new Cargo();
    private string _pickupAddress;
    private DateTime _takeDateTime;
    private string _destinationAddress;
    private DateTime _destinationDateTime;
    private OrderStatus _statusDb;
    private Courier _courier;
    private string _cancelComment;
    
    
    public Guid Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public DateTime CreateDateTime { get => _createDateTime; set { _createDateTime = value; OnPropertyChanged(); } }
    public OrderStatus Status { get => _statusDb; set { _statusDb = value; OnPropertyChanged(); } }
    public Client Client { get => _client; set { _client  = value; OnPropertyChanged(); } }
    public Cargo Cargo { get => _cargo; set { _cargo = value; OnPropertyChanged(); } }
    public string TakeAddress { get => _pickupAddress; set { _pickupAddress = value; OnPropertyChanged(); } }
    public DateTime TakeDateTime { get => _takeDateTime; set { _takeDateTime = value; OnPropertyChanged(); } }
    public Courier Courier { get => _courier; set { _courier = value; OnPropertyChanged(); } }
    public string DestinationAddress { get => _destinationAddress; set { _destinationAddress = value; OnPropertyChanged(); } }
    public DateTime DestinationDateTime { get => _destinationDateTime; set { _destinationDateTime = value; OnPropertyChanged(); } }
    public bool Deleted { get; init; } = false;
    public string CancelComment { get => _cancelComment; set { _cancelComment = value; OnPropertyChanged(); } }
    

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}