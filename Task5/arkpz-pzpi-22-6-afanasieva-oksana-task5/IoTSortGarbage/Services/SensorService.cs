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
    /// <summary>
    /// Service for interacting with sensor-related API endpoints
    /// </summary>
    public class SensorService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the SensorService class
        /// </summary>
        /// <param name="httpClient">The HTTP client for making API requests</param>
        /// <param name="token">The authentication token for API requests</param>
        public SensorService(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Retrieves all sensor IDs from the API
        /// </summary>
        /// <returns>A list of sensor IDs</returns>
        public async Task<List<int>> GetAllSensorIdAsync()
        {
            var response = await _httpClient.GetAsync("/ids");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(json);
        }

        /// <summary>
        /// Gets the maximum size of a container for a specific sensor
        /// </summary>
        /// <param name="sensorId">The ID of the sensor</param>
        /// <returns>The maximum size of the container</returns>
        public async Task<int> GetMaxSizeBySensorIdAsync(int sensorId)
        {
            var response = await _httpClient.GetAsync($"/sensors/{sensorId}/container");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(json);
        }
    }

    /// <summary>
    /// Represents a sensor device that monitors a container
    /// </summary>
    public class Sensor
    {
        public int SensorId { get; set; }
        public int ContainerId { get; set; }
    }
}