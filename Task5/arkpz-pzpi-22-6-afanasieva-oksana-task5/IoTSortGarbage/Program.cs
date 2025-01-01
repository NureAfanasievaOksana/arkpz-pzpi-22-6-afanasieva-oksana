using IoTSortGarbage.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var httpClient = new HttpClient { BaseAddress = new Uri(configuration["ApiAddress"]) };
        var authService = new AuthService(httpClient);
        var token = await authService.GetJwtTokenAsync(configuration["Auth:Email"], configuration["Auth:Password"]);

        var mqttService = new MqttService(configuration);
        await mqttService.ConnectAsync();

        try
        {
            var sendInterval = int.Parse(configuration["MqttSettings:SendInterval"]);
            var sensorService = new SensorService(httpClient, token);
            var sensorDataService = new SensorDataService(httpClient);

            while (true)
            {
                var sensorIds = await sensorService.GetAllSensorIdAsync();

                foreach (var sensorId in sensorIds)
                {
                    var maxSize = await sensorService.GetMaxSizeBySensorIdAsync(sensorId);
                    var sensorData = SensorDataService.ProcessSensorData(sensorId, maxSize);

                    await sensorDataService.SaveSensorDataAsync(sensorData);
                    Console.WriteLine($"Sensor data for Sensor ID {sensorId} has been saved: {sensorData}");

                    await mqttService.PublishAsync(sensorData);
                }
                Thread.Sleep(sendInterval);
            }
        }
        finally
        {
            await mqttService.DisconnectAsync();
        }
    }
}