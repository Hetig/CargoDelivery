using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using CargoDelivery.Client.Enums;
using CargoDelivery.Client.Models;
using CargoDelivery.Client.Models.Queries;
using Order = CargoDelivery.Client.Models.Order;

namespace CargoDelivery.Client.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:8080/api/v1/");
    }

    public async Task<PaginatedResponse<Order>> GetOrdersAsync(int pageNumber = 1, int pageSize = 10)
    {
        var response = await _httpClient.GetFromJsonAsync<PaginatedResponse<Order>>(
            $"orders?pageNumber={pageNumber}&pageSize={pageSize}");
        return response;
    }

    public async Task<Order> GetOrderByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Order>($"orders/{id}");
    }

    public async Task<Order> CreateOrderAsync(CreateOrder order)
    {
        var response = await _httpClient.PostAsJsonAsync("orders", order);
        return await response.Content.ReadFromJsonAsync<Order>();
    }

    public async Task<bool> UpdateOrderAsync(EditOrder order)
    {
        var response = await _httpClient.PutAsJsonAsync("orders", order);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Order>> SearchOrdersAsync(string query)
    {
        var response = await _httpClient.GetFromJsonAsync<PaginatedResponse<Order>>($"orders/search?query={query}");
        return response.Data;
    }

    public async Task<bool> DeleteOrderAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"orders/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AssignToCourierAsync(Guid orderId, Guid courierId)
    {
        var response = await _httpClient.PatchAsync($"orders/{orderId}/assign/{courierId}", null);
        return response.IsSuccessStatusCode;
    } 

    public async Task<bool> SetInProcessAsync(Guid orderId)
    {
        var response = await _httpClient.PatchAsync(
            $"orders/{orderId}/inprocess", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SetDoneAsync(Guid orderId)
    {
        var response = await _httpClient.PatchAsync(
            $"orders/{orderId}/done", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SetCancelAsync(Guid orderId, string comment)
    {
        var response = await _httpClient.PatchAsync(
            $"orders/{orderId}/cancel?comment={comment}", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Models.Client>> GetClientsAsync()
    {
        var clients = await _httpClient.GetFromJsonAsync<List<Models.Client>>($"clients");
        return clients;
    }

    public async Task<List<Courier>> GetCouriersAsync()
    {
        var couriers = await _httpClient.GetFromJsonAsync<List<Models.Courier>>($"couriers");
        return couriers;
    }

    public async Task<Models.Client> CreateClientAsync(CreateClient client)
    {
        var response = await _httpClient.PostAsJsonAsync($"clients", client);
        return await response.Content.ReadFromJsonAsync<Models.Client>();
    }

    public async Task<Courier> CreateCourierAsync(CreateCourier courier)
    {
        var response = await _httpClient.PostAsJsonAsync($"couriers", courier);
        return await response.Content.ReadFromJsonAsync<Courier>();
    }
}