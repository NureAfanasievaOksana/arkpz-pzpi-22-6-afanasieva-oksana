using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Controllers
{
    [Route("/sensors")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly SortGarbageDbContext _dbContext;

        public SensorController(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetSensors()
        {
            var sensors = await _dbContext.Sensors.ToListAsync();
            return Ok(sensors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSensor([FromBody] Sensor sensor)
        {
            _dbContext.Sensors.Add(sensor);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSensorById), new { id = sensor.SensorId }, sensor);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensorById(int id)
        {
            var sensor = await _dbContext.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }
            return Ok(sensor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSensor(int id, [FromBody] Sensor updatedData)
        {
            var sensor = await _dbContext.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }

            sensor.Type = updatedData.Type;
            _dbContext.Sensors.Update(sensor);
            await _dbContext.SaveChangesAsync();
            return Ok("Sensor updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            var sensor = await _dbContext.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }

            _dbContext.Sensors.Remove(sensor);
            await _dbContext.SaveChangesAsync();
            return Ok("Sensor deleted successfully");
        }
    }
}
