using Microsoft.AspNetCore.Mvc;
using SortGarbageAPI.Models;
using SortGarbageAPI.Services;

namespace SortGarbageAPI.Controllers
{
    [ApiController]
    [Route("/containers")]
    public class ContainerController : ControllerBase
    {
        private readonly ContainerService _containerService;

        public ContainerController(ContainerService containerService)
        {
            _containerService = containerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContainers()
        {
            var containers = await _containerService.GetAllContainersAsync();
            return Ok(containers);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContainer([FromBody] Container container)
        {
            var createdContainer = await _containerService.CreateContainerAsync(container);
            return CreatedAtAction(nameof(GetContainerById), new { id = createdContainer.ContainerId }, createdContainer);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContainerById(int id)
        {
            var container = await _containerService.GetContainerByIdAsync(id);
            if (container == null)
            {
                return NotFound();
            }
            return Ok(container);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContainer(int id, [FromBody] Container updatedData)
        {
            var success = await _containerService.UpdateContainerAsync(id, updatedData);
            if (!success)
            {
                return NotFound();
            }
            return Ok("Container data updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContainer(int id)
        {
            var success = await _containerService.DeleteContainerAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok("Container data deleted successfully");
        }
    }
}
