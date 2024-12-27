using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Services
{
    /// <summary>
    /// Service for managing sensors
    /// </summary>
    public class SensorService
    {
        private readonly SortGarbageDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the SensorService
        /// </summary>
        /// <param name="dbContext">Database context for sensor operations</param>
        public SensorService(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all sensors from the database
        /// </summary>
        /// <returns>Collection of all sensors</returns>
        public async Task<List<Sensor>> GetAllSensorsAsync()
        {
            return await _dbContext.Sensors.ToListAsync();
        }

        /// <summary>
        /// Creates a new sensor in the database
        /// </summary>
        /// <param name="sensor">The sensor to create</param>
        /// <returns>The created sensor with assigned id</returns>
        public async Task<Sensor> CreateSensorAsync(Sensor sensor)
        {
            _dbContext.Sensors.Add(sensor);
            await _dbContext.SaveChangesAsync();
            return sensor;
        }

        /// <summary>
        /// Retrieves a specific sensor by its id
        /// </summary>
        /// <param name="id">The sensor id</param>
        /// <returns>The sensor if found, null otherwise</returns>
        public async Task<Sensor?> GetSensorByIdAsync(int id)
        {
            return await _dbContext.Sensors.FindAsync(id);
        }

        /// <summary>
        /// Updates an existing sensor in the database
        /// </summary>
        /// <param name="id">The ID of the sensor to update</param>
        /// <param name="updatedData">The updated sensor data</param>
        /// <returns>True if update was successful, false if sensor wasn't found</returns>
        public async Task<bool> UpdateSensorAsync(int id, Sensor updatedData)
        {
            var sensor = await _dbContext.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return false;
            }

            _dbContext.Sensors.Update(sensor);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a sensor from the database
        /// </summary>
        /// <param name="id">The id of the sensor to delete</param>
        /// <returns>True if deletion was successful, false if sensor wasn't found</returns>
        public async Task<bool> DeleteSensorAsync(int id)
        {
            var sensor = await _dbContext.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return false;
            }

            _dbContext.Sensors.Remove(sensor);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}