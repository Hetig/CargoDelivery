using System.Windows.Input;
using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Models.Queries;
using CargoDelivery.Client.Services;

namespace CargoDelivery.Client.ViewModels;

public class CreateCourierViewModel : ViewModelBase
{
    public IApiService _apiService;
    private CreateCourier _createCourier;
    public ICommand SaveCommand { get; }
    private Views.CreateCourier View { get; }
    
    public CreateCourierViewModel(IApiService apiService, Views.CreateCourier view)
    {
        View = view;
        _apiService = apiService;
        SaveCommand = new RelayCommand(async () => await Save(), CanSave);
    }
    
    private async Task Save()
    {
        var newCourier = new CreateCourier() { Name = View.NameTextBox.Text };
        var createdClient = await _apiService.CreateCourierAsync(newCourier);
        View.Close();
    }

    private bool CanSave()
    {
        if (View.NameTextBox.Text == null) return false;
        if (string.IsNullOrWhiteSpace(View.NameTextBox.Text) || View.NameTextBox.Text.Length <= 3) return false;
        return true;
    }
}