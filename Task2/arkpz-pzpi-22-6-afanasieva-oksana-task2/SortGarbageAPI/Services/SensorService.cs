using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Services
{
    public class SensorService
    {
        private readonly SortGarbageDbContext _dbContext;

        public SensorService(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Sensor>> GetAllSensorsAsync()
        {
            return await _dbContext.Sensors.ToListAsync();
        }

        public async Task<Sensor?> GetSensorByIdAsync(int id)
        {
            return await _dbContext.Sensors.FindAsync(id);
        }

        public async Task<Sensor> CreateSensorAsync(Sensor sensor)
        {
            _dbContext.Sensors.Add(sensor);
            await _dbContext.SaveChangesAsync();
            return sensor;
        }

        public async Task<bool> UpdateSensorAsync(int id, Sensor updatedData)
        {
            var sensor = await _dbContext.Sensors.FindAsync(id);
            if (sensor == null) return false;

            sensor.Type = updatedData.Type;
            _dbContext.Sensors.Update(sensor);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSensorAsync(int id)
        {
            var sensor = await _dbContext.Sensors.FindAsync(id);
            if (sensor == null) return false;

            _dbContext.Sensors.Remove(sensor);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
