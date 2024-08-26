using Entities.Models;
using ProxyService.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProxyService
{
    public class ProductProxy : IProductProxy
    {
        private readonly HttpClient _httpClient;

        public ProductProxy()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7128/api/Product/") // Actualiza la URL base según tu configuración
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Product> CreateAsync(Product product)
        {
            try
            {
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Product>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                // Registra el error en lugar de solo imprimirlo en la consola
                Console.WriteLine($"Error: {ex.Message}");
                // También podrías lanzar una excepción personalizada
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Product>(); // Devuelve una lista vacía en lugar de null
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Product>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateAsync(int id, Product product)
        {
            try
            {
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Considera lanzar una excepción más específica
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
