using System.Windows.Input;
using CargoDelivery.Client.Commands;
using CargoDelivery.Client.Models.Queries;
using CargoDelivery.Client.Services;

namespace CargoDelivery.Client.ViewModels;

public class CreateClientViewModel : ViewModelBase
{
    public IApiService _apiService;
    private CreateClient _createClient;
    private Views.CreateClient View { get; }
    public ICommand SaveCommand { get; }
    
    public CreateClientViewModel(IApiService apiService, Views.CreateClient view)
    {
        View = view;
        _apiService = apiService;
        SaveCommand = new RelayCommand(async () => await Save(), CanSave);
    }
    
    private async Task Save()
    {
        var newClient = new CreateClient() { Name = View.NameTextBox.Text };
        var createdClient = await _apiService.CreateClientAsync(newClient);
        View.Close();
    }

    private bool CanSave()
    {
        if (View.NameTextBox.Text == null) return false;
        if (string.IsNullOrWhiteSpace(View.NameTextBox.Text) || View.NameTextBox.Text.Length <= 3) return false;
        return true;
    }
}