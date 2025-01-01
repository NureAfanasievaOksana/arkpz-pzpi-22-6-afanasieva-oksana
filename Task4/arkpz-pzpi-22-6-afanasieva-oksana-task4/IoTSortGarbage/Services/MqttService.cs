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
    /// <summary>
    /// Service for handling MQTT communication
    /// </summary>
    public class MqttService
    {
        private readonly IMqttClient _mqttClient;
        private readonly string _brokerAddress;
        private readonly int _brokerPort;
        private readonly string _sensorData;

        /// <summary>
        /// Initializes a new instance of the MqttService class
        /// </summary>
        /// <param name="configuration">The configuration containing MQTT settings</param>
        public MqttService(IConfiguration configuration)
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var mqttSettings = configuration.GetSection("MqttSettings");
            _brokerAddress = mqttSettings["Broker"];
            _brokerPort = int.Parse(mqttSettings["Port"]);
            _sensorData = mqttSettings["SensorData"];
        }

        /// <summary>
        /// Establishes connection to the MQTT broker
        /// </summary>
        /// <returns>A task that represents the asynchronous connection operation</returns>
        public async Task ConnectAsync()
        {
            if (!_mqttClient.IsConnected)
            {
                var mqttOptions = new MqttClientOptionsBuilder().WithTcpServer(_brokerAddress, _brokerPort).Build();
                await _mqttClient.ConnectAsync(mqttOptions);
                Console.WriteLine("Connected to MQTT broker.");
            }
        }

        /// <summary>
        /// Publishes sensor data to the MQTT broker
        /// </summary>
        /// <param name="sensorData">The sensor data to publish</param>
        /// <returns>A task that represents the asynchronous publish operation</returns>
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

        /// <summary>
        /// Disconnects from the MQTT broker
        /// </summary>
        /// <returns>A task that represents the asynchronous disconnection operation</returns>
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