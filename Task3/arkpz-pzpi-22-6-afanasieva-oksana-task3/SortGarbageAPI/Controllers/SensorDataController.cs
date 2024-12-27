using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SortGarbageAPI.Models;
using SortGarbageAPI.Services;

namespace SortGarbageAPI.Controllers
{
    /// <summary>
    /// Controller responsible for managing sensor data
    /// </summary>
    [Route("/sensorData")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly SensorDataService _sensorDataService;

        /// <summary>
        /// Initializes a new instance of the SensorDataController class
        /// </summary>
        /// <param name="sensorDataService">The service for managing sensor data operations</param>
        public SensorDataController(SensorDataService sensorDataService)
        {
            _sensorDataService = sensorDataService;
        }

        /// <summary>
        /// Retrieves all sensor data from the system
        /// </summary>
        /// <returns>A collection of all sensor data</returns>
        [HttpGet]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetSensorData()
        {
            var sensorData = await _sensorDataService.GetAllSensorDataAsync();
            return Ok(sensorData);
        }

        /// <summary>
        /// Creates a new sensor data entry in the system
        /// </summary>
        /// <param name="sensorData">The sensor data to create</param>
        /// <returns>The created sensor data</returns>
        [HttpPost]
        //[Authorize(Roles = "2, 3")]
        public async Task<IActionResult> CreateSensorData([FromBody] SensorData sensorData)
        {
            var createdSensorData = await _sensorDataService.CreateSensorDataAsync(sensorData);
            return CreatedAtAction(nameof(GetSensorDataById), new { id = createdSensorData.SensorDataId }, createdSensorData);
        }

        /// <summary>
        /// Retrieves specific sensor data by its id
        /// </summary>
        /// <param name="id">The id of the sensor data to retrieve</param>
        /// <returns>The requested sensor data</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetSensorDataById(int id)
        {
            var sensorData = await _sensorDataService.GetSensorDataByIdAsync(id);
            if (sensorData == null)
            {
                return NotFound();
            }
            return Ok(sensorData);
        }

        /// <summary>
        /// Deletes sensor data from the system
        /// </summary>
        /// <param name="id">The id of the sensor data to delete</param>
        /// <returns>A success message if the deletion was successful</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> DeleteSensorData(int id)
        {
            if (!await _sensorDataService.DeleteSensorDataAsync(id))
            {
                return NotFound();
            }
            return Ok("Sensor data deleted successfully");
        }

        /// <summary>
        /// Retrieves all sensor data for a specific sensor.
        /// </summary>
        /// <param name="sensorId">The id of the sensor to retrieve data for.</param>
        /// <returns>A collection of sensor data for the specified sensor.</returns>
        [HttpGet("sensor/{sensorId}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetSensorDataBySensorId(int sensorId)
        {
            var sensorData = await _sensorDataService.GetSensorDataBySensorIdAsync(sensorId);
            return Ok(sensorData);
        }

        /// <summary>
        /// Retrieves all sensor data for a specific date.
        /// </summary>
        /// <param name="date">The date to retrieve sensor data for.</param>
        /// <returns>A collection of sensor data for the specified date.</returns>
        [HttpGet("date/{date}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetSensorDataByDate(DateTime date)
        {
            var sensorData = await _sensorDataService.GetSensorDataByDateAsync(date);
            return Ok(sensorData);
        }

    }
}