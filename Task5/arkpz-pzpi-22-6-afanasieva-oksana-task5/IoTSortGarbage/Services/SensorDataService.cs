using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace IoTSortGarbage.Services
{
    /// <summary>
    /// Service for managing and processing sensor data
    /// </summary>
    public class SensorDataService
    {
        private readonly HttpClient _httpClient;
        private static Random _random = new Random();

        /// <summary>
        /// Initializes a new instance of the SensorDataService class
        /// </summary>
        /// <param name="httpClient">The HTTP client for making API requests</param>
        public SensorDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Generates a random fill level for a container
        /// </summary>
        /// <param name="maxSize">The maximum size of the container</param>
        /// <returns>A random fill level value</returns>
        public static float GenerateFillLevel(float maxSize)
        {
            return (float)Math.Round((float)_random.NextDouble() * maxSize, 2);
        }

        /// <summary>
        /// Generates a random temperature value between -20 and 60 degrees
        /// </summary>
        /// <returns>A random temperature value</returns>
        public static float GenerateTemperature()
        {
            return (float)Math.Round((float)_random.NextDouble() * 80 - 20, 2);
        }

        /// <summary>
        /// Generates a random humidity value between 0 and 30 percent
        /// </summary>
        /// <returns>A random humidity value</returns>
        public static float GenerateHumidity()
        {
            return (float)Math.Round((float)_random.NextDouble() * 30, 2);
        }

        /// <summary>
        /// Processes sensor data by generating measurements and calculating fullness
        /// </summary>
        /// <param name="sensorId">The ID of the sensor</param>
        /// <param name="maxSize">The maximum size of the container</param>
        /// <returns>An object containing processed sensor data</returns>
        public static object ProcessSensorData(int sensorId, float maxSize)
        {
            if (maxSize <= 0)
            {
                throw new ArgumentException("Max volume must be greater than zero.");
            }

            float filledVolume = GenerateFillLevel(maxSize);
            float temperature = GenerateTemperature();
            float humidity = GenerateHumidity();
            float fullness = (filledVolume / maxSize) * 100;

            return new
            {
                Timestamp = DateTime.UtcNow,
                Fullness = fullness,
                Temperature = temperature,
                Wetness = humidity,
                SensorId = sensorId
            };
        }

        /// <summary>
        /// Saves sensor data to the API
        /// </summary>
        /// <param name="sensorData">The sensor data to save</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task SaveSensorDataAsync(object sensorData)
        {
            var json = JsonSerializer.Serialize(sensorData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/sensorData", content);
            response.EnsureSuccessStatusCode();
        }
    }
}