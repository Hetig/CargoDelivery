using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using CargoDelivery.Client.Enums;
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

    public async Task<Order> CreateOrderAsync(Order order)
    {
        var response = await _httpClient.PostAsJsonAsync("orders", order);
        return await response.Content.ReadFromJsonAsync<Order>();
    }

    public async Task<bool> UpdateOrderAsync(Order order)
    {
        var response = await _httpClient.PutAsJsonAsync("orders", order);
        return response.IsSuccessStatusCode;
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

    public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status, string comment = null)
    {
       var endpoint = status switch
        {
            OrderStatus.InProcess => $"orders/{orderId}/inprocess",
            OrderStatus.Done => $"orders/{orderId}/done",
            OrderStatus.Cancelled => $"orders/{orderId}/cancel?comment={comment}",
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };

        var response = await _httpClient.PatchAsync(endpoint, null);
        return response.IsSuccessStatusCode;
    }
}