using CargoDelivery.Client.ViewModels;

namespace CargoDelivery.Client.Services;

public class NavigationService : INavigationService
{
    private readonly Func<Type, ViewModelBase> _viewModelFactory;
    private ViewModelBase _currentView;

    public ViewModelBase CurrentView
    {
        get => _currentView;
        private set
        {
            _currentView = value;
            ViewChanged?.Invoke();
        }
    }

    public event Action ViewChanged;

    public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<T>() where T : ViewModelBase
    {
        CurrentView = _viewModelFactory(typeof(T));
    }

    public void NavigateTo(ViewModelBase viewModel)
    {
        CurrentView = viewModel;
    }
}