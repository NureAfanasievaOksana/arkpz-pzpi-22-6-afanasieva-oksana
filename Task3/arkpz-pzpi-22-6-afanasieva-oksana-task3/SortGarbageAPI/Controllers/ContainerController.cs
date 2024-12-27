using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SortGarbageAPI.Models;
using SortGarbageAPI.Services;

namespace SortGarbageAPI.Controllers
{
    /// <summary>
    /// Controller responsible for managing garbage containers
    /// </summary>
    [ApiController]
    [Route("/containers")]
    public class ContainerController : ControllerBase
    {
        private readonly ContainerService _containerService;

        /// <summary>
        /// Initializes a new instance of the ContainerController class
        /// </summary>
        /// <param name="containerService">The service for managing container operations</param>
        public ContainerController(ContainerService containerService)
        {
            _containerService = containerService;
        }

        /// <summary>
        /// Retrieves all containers from the system
        /// </summary>
        /// <returns>A collection of all containers</returns>
        [HttpGet]
        [Authorize(Roles = "1, 2, 3")]
        public async Task<IActionResult> GetContainers()
        {
            var containers = await _containerService.GetAllContainersAsync();
            return Ok(containers);
        }

        /// <summary>
        /// Creates a new container in the system
        /// </summary>
        /// <param name="container">The container data to create</param>
        /// <returns>The created container data</returns>
        [HttpPost]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> CreateContainer([FromBody] Container container)
        {
            var createdContainer = await _containerService.CreateContainerAsync(container);
            return CreatedAtAction(nameof(GetContainerById), new { id = createdContainer.ContainerId }, createdContainer);
        }

        /// <summary>
        /// Retrieves a specific container by its id
        /// </summary>
        /// <param name="id">The id of the container to retrieve</param>
        /// <returns>The requested container data</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetContainerById(int id)
        {
            var container = await _containerService.GetContainerByIdAsync(id);
            if (container == null)
            {
                return NotFound();
            }
            return Ok(container);
        }

        /// <summary>
        /// Updates an existing container's information
        /// </summary>
        /// <param name="id">The id of the container to update</param>
        /// <param name="updatedData">The updated container data</param>
        /// <returns>A success message if the update was successful</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> UpdateContainer(int id, [FromBody] Container updatedData)
        {
            if (!await _containerService.UpdateContainerAsync(id, updatedData))
            {
                return NotFound();
            }
            return Ok("Container data updated successfully");
        }

        /// <summary>
        /// Deletes a container from the system
        /// </summary>
        /// <param name="id">The id of the container to delete</param>
        /// <returns>A success message if the deletion was successful</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> DeleteContainer(int id)
        {
            if (!await _containerService.DeleteContainerAsync(id))
            {
                return NotFound();
            }
            return Ok("Container data deleted successfully");
        }

        /// <summary>
        /// Retrieves all containers of a specific type
        /// </summary>
        /// <param name="type">The type of containers to retrieve</param>
        /// <returns>A collection of containers of the specified type</returns>
        [HttpGet("type/{type}")]
        [Authorize(Roles = "1, 2, 3")]
        public async Task<IActionResult> GetContainersByType(string type)
        {
            if (!Enum.TryParse<ContainerType>(type, true, out var containerType))
            {
                return BadRequest("Invalid container type");
            }
            var containers = await _containerService.GetContainersByTypeAsync(containerType);
            return Ok(containers);
        }

        /// <summary>
        /// Retrieves all containers at a specific address
        /// </summary>
        /// <param name="address">The address to retrieve containers for</param>
        /// <returns>A collection of containers at the specified address</returns>
        [HttpGet("address/{address}")]
        [Authorize(Roles = "1, 2, 3")]
        public async Task<IActionResult> GetContainersByAddress(string address)
        {
            var containers = await _containerService.GetContainersByAddressAsync(address);
            return Ok(containers);
        }
    }
}