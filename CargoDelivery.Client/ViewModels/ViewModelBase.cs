using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CargoDelivery.Client.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    protected void OnPropertyChanged<T>(Expression<Func<T>> action)
    {
        var propertyName = GetPropertyName(action);
        this.OnPropertyChanged(propertyName);
    }
    private static string GetPropertyName<T>(Expression<Func<T>> action)
    {
        var expression = (MemberExpression)action.Body;
        var propertyName = expression.Member.Name;
        return propertyName;
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = this.PropertyChanged;
        if (handler != null)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
}