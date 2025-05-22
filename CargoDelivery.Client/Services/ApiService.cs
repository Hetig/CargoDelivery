using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using CargoDelivery.Domain.Models;
using Order = CargoDelivery.Client.Models.Order;

namespace CargoDelivery.Client.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:5074/api/v1/";

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_baseUrl);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("orders");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Order>>();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Возникла ошибка получении всех заказов", "Ошибка");
            throw;
        }
    }
    
    public async Task<Order> GetOrderByIdAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"orders/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Order>();
        }
        catch (Exception e)
        {
            MessageBox.Show("Возникла ошибка при поиске заказа", "Ошибка");
            throw;
        }
    }

    public async Task<IEnumerable<Order>> SearchOrdersAsync(string query)
    {
        try
        {
            var response = await _httpClient.GetAsync($"orders/search?query={Uri.EscapeDataString(query)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Order>>();
        }
        catch (Exception e)
        {
            MessageBox.Show("Возникла ошибка при поиске заказа(ов)", "Ошибка");
            return null;
        }
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(order),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("orders", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Order>();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Невозможно создать заказ", "Ошибка");
            return null;
        }
    }

    public async Task UpdateOrderAsync(Order order)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(order),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"Orders/update", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Невозможно обновить заказ", "Ошибка");
        }
    }

    public async Task OrderDoneAsync(Order order)
    {
        try
        {
            var orderId = order.Id.ToString();
            var response = await _httpClient.PostAsync($"orders/done/{orderId}", null);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Невозможно обновить заказ", "Ошибка");
        }
    }

    public async Task OrderInProcessAsync(Order order)
    {
        try
        {
            var orderId = order.Id.ToString();
            var response = await _httpClient.PostAsync($"orders/inprocess/{orderId}", null);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Невозможно обновить заказ", "Ошибка");
        }
    }

    public async Task OrderCancelAsync(Order order)
    {
        try
        {
            var orderId = order.Id.ToString();
            var response = await _httpClient.PostAsync($"orders/cancel/{orderId}", null);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Невозможно обновить заказ", "Ошибка");
        }
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"orders/delete/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Невозможно удалить заказ", "Ошибка");
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("couriers");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Courier>>();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Невозможно получить курьеров", "Ошибка");
            return null;
        }
    }
}