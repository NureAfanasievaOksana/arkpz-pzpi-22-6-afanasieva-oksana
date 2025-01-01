using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace IoTSortGarbage.Services
{
    public class SensorDataService
    {
        private readonly HttpClient _httpClient;
        private static Random _random = new Random();

        public SensorDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public static float GenerateFillLevel(float maxSize)
        {
            return (float)Math.Round((float)_random.NextDouble() * maxSize, 2);
        }
        public static float GenerateTemperature()
        {
            return (float)Math.Round((float)_random.NextDouble() * 80 - 20, 2);
        }
        public static float GenerateHumidity()
        {
            return (float)Math.Round((float)_random.NextDouble() * 30, 2);
        }

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

        public async Task SaveSensorDataAsync(object sensorData)
        {
            var json = JsonSerializer.Serialize(sensorData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/sensorData", content);
            response.EnsureSuccessStatusCode();
        }
    }
}