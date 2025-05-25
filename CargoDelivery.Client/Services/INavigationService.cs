using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Services;

public interface INavigationService
{
    ViewModelBase CurrentView { get; }
    void NavigateTo<T>() where T : ViewModelBase;
    void NavigateTo(ViewModelBase viewModel);
    event Action ViewChanged;
}