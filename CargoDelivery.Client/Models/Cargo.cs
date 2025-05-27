using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CargoDelivery.Client.Models;

public class Cargo : INotifyPropertyChanged
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}