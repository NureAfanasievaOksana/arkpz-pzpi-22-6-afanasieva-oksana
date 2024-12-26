using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Services
{
    public class SensorDataService
    {
        private readonly SortGarbageDbContext _dbContext;

        public SensorDataService(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SensorData>> GetAllSensorDataAsync()
        {
            return await _dbContext.SensorData.ToListAsync();
        }

        public async Task<SensorData?> GetSensorDataByIdAsync(int id)
        {
            return await _dbContext.SensorData.FindAsync(id);
        }

        public async Task<SensorData> CreateSensorDataAsync(SensorData sensorData)
        {
            _dbContext.SensorData.Add(sensorData);
            await _dbContext.SaveChangesAsync();
            return sensorData;
        }

        public async Task<bool> DeleteSensorDataAsync(int id)
        {
            var sensorData = await _dbContext.SensorData.FindAsync(id);
            if (sensorData == null) return false;

            _dbContext.SensorData.Remove(sensorData);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
