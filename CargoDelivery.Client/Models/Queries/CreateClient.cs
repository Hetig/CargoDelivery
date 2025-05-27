using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CargoDelivery.Client.Models.Queries;

public class CreateClient : INotifyPropertyChanged
{
    public string Name { get; set; }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}