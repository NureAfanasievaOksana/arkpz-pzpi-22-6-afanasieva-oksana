﻿namespace SortGarbageAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SortGarbageAPI.Models;
    using SortGarbageAPI.Services;

    /// <summary>
    /// Controller responsible for managing sensors
    /// </summary>
    [Route("/sensors")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        /// <summary>
        /// Defines the _sensorService
        /// </summary>
        private readonly SensorService _sensorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorController"/> class.
        /// </summary>
        /// <param name="sensorService">The service for managing sensor operations</param>
        public SensorController(SensorService sensorService)
        {
            _sensorService = sensorService;
        }

        /// <summary>
        /// Retrieves all sensors from the system
        /// </summary>
        /// <returns>A collection of all sensors</returns>
        [HttpGet]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetSensors()
        {
            var sensors = await _sensorService.GetAllSensorsAsync();
            return Ok(sensors);
        }

        /// <summary>
        /// Creates a new sensor in the system
        /// </summary>
        /// <param name="sensor">The sensor data to create</param>
        /// <returns>The created sensor data</returns>
        [HttpPost]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> CreateSensor([FromBody] Sensor sensor)
        {
            var createdSensor = await _sensorService.CreateSensorAsync(sensor);
            return CreatedAtAction(nameof(GetSensorById), new { id = createdSensor.SensorId }, createdSensor);
        }

        /// <summary>
        /// Retrieves a specific sensor by its id
        /// </summary>
        /// <param name="id">The ID of the sensor to retrieve</param>
        /// <returns>The requested sensor data</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetSensorById(int id)
        {
            var sensor = await _sensorService.GetSensorByIdAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }
            return Ok(sensor);
        }

        /// <summary>
        /// Retrieves the maximum size of the container associated with a specific sensor
        /// </summary>
        /// <param name="id">The ID of the sensor whose container information is to be retrieved</param>
        /// <returns>The maximum size of the container if found, or NotFound if the sensor or container doesn't exist</returns>
        [HttpGet("{id}/container")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetContainerBySensorId(int id)
        {
            var maxSize = await _sensorService.GetContainerBySensorIdAsync(id);
            if (maxSize == null)
            {
                return NotFound();
            }
            return Ok(maxSize);
        }

        /// <summary>
        /// Retrieves a list of all sensor IDs in the system
        /// </summary>
        /// <returns>A collection of sensor IDs</returns>
        [HttpGet("/ids")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetAllSensorsId()
        {
            var sensorIds = await _sensorService.GetAllSensorIdsAsync();
            return Ok(sensorIds);
        }

        /// <summary>
        /// Updates an existing sensor's information
        /// </summary>
        /// <param name="id">The id of the sensor to update</param>
        /// <param name="updatedData">The updated sensor data</param>
        /// <returns>A success message if the update was successful</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> UpdateSensor(int id, [FromBody] Sensor updatedData)
        {
            if (!await _sensorService.UpdateSensorAsync(id, updatedData))
            {
                return NotFound();
            }
            return Ok("Sensor updated successfully");
        }

        /// <summary>
        /// Deletes a sensor from the system
        /// </summary>
        /// <param name="id">The id of the sensor to delete</param>
        /// <returns>A success message if the deletion was successful</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            if (!await _sensorService.DeleteSensorAsync(id))
            {
                return NotFound();
            }
            return Ok("Sensor deleted successfully");
        }
    }
}