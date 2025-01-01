using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;

namespace IoTSortGarbage.Services
{
    public class SensorService
    {
        private readonly HttpClient _httpClient;
        public SensorService(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<int>> GetAllSensorIdAsync()
        {
            var response = await _httpClient.GetAsync("/ids");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(json);
        }

        public async Task<int> GetMaxSizeBySensorIdAsync(int sensorId)
        {
            var response = await _httpClient.GetAsync($"/sensors/{sensorId}/container");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(json);
        }
    }

    public class Sensor
    {
        public int SensorId { get; set; }
        public int ContainerId { get; set; }
    }
}