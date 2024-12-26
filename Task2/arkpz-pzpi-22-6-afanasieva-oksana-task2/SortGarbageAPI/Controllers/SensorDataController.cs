using Microsoft.AspNetCore.Mvc;
using SortGarbageAPI.Models;
using SortGarbageAPI.Services;

namespace SortGarbageAPI.Controllers
{
    [Route("/sensorData")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly SensorDataService _sensorDataService;

        public SensorDataController(SensorDataService sensorDataService)
        {
            _sensorDataService = sensorDataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSensorData()
        {
            var sensorData = await _sensorDataService.GetAllSensorDataAsync();
            return Ok(sensorData);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSensorData([FromBody] SensorData sensorData)
        {
            var createdSensorData = await _sensorDataService.CreateSensorDataAsync(sensorData);
            return CreatedAtAction(nameof(GetSensorDataById), new { id = createdSensorData.SensorDataId }, createdSensorData);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensorDataById(int id)
        {
            var sensorData = await _sensorDataService.GetSensorDataByIdAsync(id);
            if (sensorData == null)
            {
                return NotFound();
            }
            return Ok(sensorData);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensorData(int id)
        {
            var success = await _sensorDataService.DeleteSensorDataAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok("Sensor data deleted successfully");
        }
    }
}
