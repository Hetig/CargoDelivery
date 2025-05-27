using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CargoDelivery.Client.Models.Queries;

public class CreateOrder : INotifyPropertyChanged
{
    private Guid _clientId;
    private CreateCargo _cargo;
    private string _takeAddress;
    private DateTime _takeDateTime;
    private string _destinationAddress;
    private DateTime _destinationDateTime;
    
    public Guid ClientId { get => _clientId; set { _clientId  = value; OnPropertyChanged(); } }
    public CreateCargo Cargo { get => _cargo; set { _cargo = value; OnPropertyChanged(); } } 
    public string TakeAddress { get => _takeAddress; set { _takeAddress = value; OnPropertyChanged(); } }
    public DateTime TakeDateTime { get => _takeDateTime; set { _takeDateTime = value; OnPropertyChanged(); } }
    public string DestinationAddress { get => _destinationAddress; set { _destinationAddress = value; OnPropertyChanged(); } }
    public DateTime DestinationDateTime { get => _destinationDateTime; set { _destinationDateTime = value; OnPropertyChanged(); } }
    
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}