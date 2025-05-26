using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Services;

public interface INavigationService
{
    void NavigateTo<T>() where T : ViewModelBase;
    void NavigateTo(ViewModelBase viewModel);
    ViewModelBase CurrentView { get; }
    event Action ViewChanged;
}