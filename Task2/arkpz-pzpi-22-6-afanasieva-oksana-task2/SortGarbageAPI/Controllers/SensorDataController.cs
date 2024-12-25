using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Controllers
{
    [Route("/sensorData")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly SortGarbageDbContext _dbContext;

        public SensorDataController(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetSensorData()
        {
            var sensorData = await _dbContext.SensorData.ToListAsync();
            return Ok(sensorData);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSensorData([FromBody] SensorData sensorData)
        {
            _dbContext.SensorData.Add(sensorData);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSensorDataById), new { id = sensorData.SensorDataId }, sensorData);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensorDataById(int id)
        {
            var sensorData = await _dbContext.SensorData.FindAsync(id);
            if (sensorData == null)
            {
                return NotFound();
            }
            return Ok(sensorData);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensorData(int id)
        {
            var sensorData = await _dbContext.SensorData.FindAsync(id);
            if (sensorData == null)
            {
                return NotFound();
            }

            _dbContext.SensorData.Remove(sensorData);
            await _dbContext.SaveChangesAsync();
            return Ok("Sensor data deleted successfully");
        }

        [HttpGet("sensor/{sensorId}")]
        public async Task<IActionResult> GetSensorDataBySensorId(int sensorId)
        {
            var sensorData = await _dbContext.SensorData.Where(sd => sd.SensorId == sensorId).ToListAsync();
            return Ok(sensorData);
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetSensorDataByDate(DateTime date)
        {
            var sensorData = await _dbContext.SensorData.Where(sd => sd.Timestamp.Date == date.Date).ToListAsync();
            return Ok(sensorData);
        }
    }
}
