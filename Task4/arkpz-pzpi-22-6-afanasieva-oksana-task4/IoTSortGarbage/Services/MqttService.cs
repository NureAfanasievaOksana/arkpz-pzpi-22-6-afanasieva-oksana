using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

namespace IoTSortGarbage.Services
{
    public class MqttService
    {
        private readonly IMqttClient _mqttClient;
        private readonly string _brokerAddress;
        private readonly int _brokerPort;
        private readonly string _sensorData;

        public MqttService(IConfiguration configuration)
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var mqttSettings = configuration.GetSection("MqttSettings");
            _brokerAddress = mqttSettings["Broker"];
            _brokerPort = int.Parse(mqttSettings["Port"]);
            _sensorData = mqttSettings["SensorData"];
        }

        public async Task ConnectAsync()
        {
            if (!_mqttClient.IsConnected)
            {
                var mqttOptions = new MqttClientOptionsBuilder().WithTcpServer(_brokerAddress, _brokerPort).Build();
                await _mqttClient.ConnectAsync(mqttOptions);
                Console.WriteLine("Connected to MQTT broker.");
            }
        }

        public async Task PublishAsync(object sensorData)
        {
            if (!_mqttClient.IsConnected)
            {
                await ConnectAsync();
            }
            var jsonPayload = System.Text.Json.JsonSerializer.Serialize(sensorData);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(_sensorData)
                .WithPayload(jsonPayload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await _mqttClient.PublishAsync(message);
        }

        public async Task DisconnectAsync()
        {
            if (_mqttClient.IsConnected)
            {
                await _mqttClient.DisconnectAsync();
            }
            Console.WriteLine("Disconnected from MQTT broker.");
        }
    }
}