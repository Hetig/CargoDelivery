using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CargoDelivery.Client.Models;

public class Client : INotifyPropertyChanged
{
    private Guid _id;
    private string _name;

    public Guid Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

    public override string ToString()
    {
        return _name;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}