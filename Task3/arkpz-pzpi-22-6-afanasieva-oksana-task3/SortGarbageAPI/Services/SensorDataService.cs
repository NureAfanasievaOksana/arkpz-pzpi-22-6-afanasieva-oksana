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

        /// <summary>
        /// Initializes a new instance of the SensorDataService
        /// </summary>
        /// <param name="dbContext">Database context for sensor data operations</param>
        public SensorDataService(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
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
        /// Creates a new sensor data record in the database
        /// </summary>
        /// <param name="sensorData">The sensor data to create</param>
        /// <returns>The created sensor data with assigned id</returns>
        public async Task<SensorData> CreateSensorDataAsync(SensorData sensorData)
        {
            _dbContext.SensorData.Add(sensorData);
            await _dbContext.SaveChangesAsync();
            return sensorData;
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