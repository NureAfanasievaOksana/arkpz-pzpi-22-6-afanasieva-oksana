using Microsoft.AspNetCore.Mvc;
using SortGarbageAPI.Models;
using SortGarbageAPI.Services;

namespace SortGarbageAPI.Controllers
{
    [Route("/sensors")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly SensorService _sensorService;

        public SensorController(SensorService sensorService)
        {
            _sensorService = sensorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSensors()
        {
            var sensors = await _sensorService.GetAllSensorsAsync();
            return Ok(sensors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSensor([FromBody] Sensor sensor)
        {
            var createdSensor = await _sensorService.CreateSensorAsync(sensor);
            return CreatedAtAction(nameof(GetSensorById), new { id = createdSensor.SensorId }, createdSensor);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensorById(int id)
        {
            var sensor = await _sensorService.GetSensorByIdAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }
            return Ok(sensor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSensor(int id, [FromBody] Sensor updatedData)
        {
            var success = await _sensorService.UpdateSensorAsync(id, updatedData);
            if (!success)
            {
                return NotFound();
            }
            return Ok("Sensor updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            var success = await _sensorService.DeleteSensorAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok("Sensor deleted successfully");
        }
    }
}
