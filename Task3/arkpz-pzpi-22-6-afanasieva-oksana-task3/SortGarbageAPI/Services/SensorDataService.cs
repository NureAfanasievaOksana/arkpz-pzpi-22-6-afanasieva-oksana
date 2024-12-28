using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Services
{
    /// <summary>
    /// Service for managing sensor data
    /// </summary>
    public class SensorDataService
    {
        private readonly SortGarbageDbContext _dbContext;
        private readonly NotificationService _notificationService;
        private readonly float _criticalFullnessThreshold;
        private readonly float _highTemperatureThreshold;
        private readonly float _highHumidityThreshold;

        /// <summary>
        /// Initializes a new instance of the SensorDataService
        /// </summary>
        /// <param name="dbContext">Database context for sensor data operations</param>
        /// <param name="notificationService">Service for sending alerts when thresholds are exceeded</param>
        /// <param name="configuration">Configuration containing sensor threshold settings</param>
        public SensorDataService(SortGarbageDbContext dbContext, NotificationService notificationService, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
            _criticalFullnessThreshold = configuration.GetValue<float>("SensorSettings:CriticalFullnessThreshold");
            _highTemperatureThreshold = configuration.GetValue<float>("SensorSettings:HighTemperatureThreshold");
            _highHumidityThreshold = configuration.GetValue<float>("SensorSettings:HighHumidityThreshold");
        }

        /// <summary>
        /// Retrieves all sensor data entries from the database
        /// </summary>
        /// <returns>Collection of all sensor data records</returns>
        public async Task<List<SensorData>> GetAllSensorDataAsync()
        {
            return await _dbContext.SensorData.ToListAsync();
        }

        /// <summary>
        /// Creates a new sensor data record and checks for threshold violations
        /// </summary>
        /// <param name="sensorData">The sensor data to create</param>
        /// <returns>The created sensor data with assigned id</returns>
        public async Task<SensorData> CreateSensorDataAsync(SensorData sensorData)
        {
            float maxAbsoluteHumidity = calculateMaxAbsoluteHumidity(sensorData.Temperature);
            float humidity = (sensorData.Wetness / maxAbsoluteHumidity) * 100;
            sensorData.Wetness = humidity;

            var sensor = await _dbContext.Sensors.Include(s => s.Container).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(s => s.SensorId == sensorData.SensorId);
            if (sensor == null)
            {
                throw new Exception("Sensor not found");
            }
            var container = sensor.Container;

            if (sensorData.Fullness >= _criticalFullnessThreshold)
            {
                var notification = new Notification
                {
                    UserId = (int)container.UserId,
                    Subject = "Critical container alert",
                    Message = $"Container {container.ContainerId} is critically full with a level of {sensorData.Fullness}%.",
                    SensorDataId = sensorData.SensorId,
                };
                await _notificationService.CreateNotificationAsync(notification);
            }

            if (sensorData.Temperature >= _highTemperatureThreshold)
            {
                var notification = new Notification
                {
                    UserId = (int)container.UserId,
                    Subject = "High Temperature Alert",
                    Message = $"Temperature in container {container.ContainerId} is too high, it is equal to {sensorData.Temperature}°C.",
                    SensorDataId = sensorData.SensorId,
                };
                await _notificationService.CreateNotificationAsync(notification);
            }

            if (sensorData.Wetness >= _highHumidityThreshold)
            {
                var notification = new Notification
                {
                    UserId = (int)container.UserId,
                    Subject = "High Humidity Alert",
                    Message = $"Relative humidity in container {container.ContainerId} is too high, it is equal to {sensorData.Wetness}%.",
                    SensorDataId = sensorData.SensorId,
                };
                await _notificationService.CreateNotificationAsync(notification);
            }

            _dbContext.SensorData.Add(sensorData);
            await _dbContext.SaveChangesAsync();
            return sensorData;
        }

        /// <summary>
        /// Calculates the maximum absolute humidity possible at a given temperature
        /// </summary>
        /// <param name="temperature">Temperature in Celsius</param>
        /// <returns>Maximum absolute humidity in g/m^3</returns>
        private float calculateMaxAbsoluteHumidity(float temperature)
        {
            double exponent = (17.67 * temperature) / (temperature + 243.5);
            double numerator = 6.112 * Math.Exp(exponent) * 216.7;
            double maxAbsoluteHumidity = numerator / (temperature + 273.15);
            return (float)maxAbsoluteHumidity;
        }

        /// <summary>
        /// Retrieves specific sensor data by its id
        /// </summary>
        /// <param name="id">The sensor data id</param>
        /// <returns>The sensor data if found, null otherwise</returns>
        public async Task<SensorData?> GetSensorDataByIdAsync(int id)
        {
            return await _dbContext.SensorData.FindAsync(id);
        }

        /// <summary>
        /// Deletes a sensor data record from the database
        /// </summary>
        /// <param name="id">The id of the sensor data to delete</param>
        /// <returns>True if deletion was successful, false if sensor data wasn't found</returns>
        public async Task<bool> DeleteSensorDataAsync(int id)
        {
            var sensorData = await _dbContext.SensorData.FindAsync(id);
            if (sensorData == null)
            {
                return false;
            }

            _dbContext.SensorData.Remove(sensorData);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves all sensor data entries for a specific sensor
        /// </summary>
        /// <param name="sensorId">The ID of the sensor to get data for</param>
        /// <returns>Collection of sensor data entries for the specified sensor</returns>
        public async Task<IEnumerable<SensorData>> GetSensorDataBySensorIdAsync(int sensorId)
        {
            return await _dbContext.SensorData.Where(sd => sd.SensorId == sensorId).ToListAsync();
        }

        /// <summary>
        /// Retrieves all sensor data entries for a specific date
        /// </summary>
        /// <param name="date">The date to get sensor data for</param>
        /// <returns>Collection of sensor data entries recorded on the specified date</returns>
        public async Task<IEnumerable<SensorData>> GetSensorDataByDateAsync(DateTime date)
        {
            return await _dbContext.SensorData.Where(sd => sd.Timestamp.Date == date.Date).ToListAsync();
        }
    }
}