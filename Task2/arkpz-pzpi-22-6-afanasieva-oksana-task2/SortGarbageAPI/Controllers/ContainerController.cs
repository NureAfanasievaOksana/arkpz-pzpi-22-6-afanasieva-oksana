using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Controllers
{
    [ApiController]
    [Route("/containers")]
    public class ContainerController : ControllerBase
    {
        private readonly SortGarbageDbContext _dbContext;

        public ContainerController(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetContainers()
        {
            var containers = await _dbContext.Containers.ToListAsync();
            return Ok(containers);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContainer([FromBody] Container container)
        {
            _dbContext.Containers.Add(container);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContainerById), new { id = container.ContainerId }, container);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContainerById(int id)
        {
            var container = await _dbContext.Containers.FindAsync(id);
            if (container == null)
            {
                return NotFound();
            }
            return Ok(container);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContainer(int id, [FromBody] Container updatedData)
        {
            var container = await _dbContext.Containers.FindAsync(id);
            if (container == null)
            {
                return NotFound();
            }

            container.Address = updatedData.Address;
            container.Type = updatedData.Type;
            container.MaxSize = updatedData.MaxSize;
            _dbContext.Containers.Update(container);
            await _dbContext.SaveChangesAsync();
            return Ok("Container data updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContainer(int id)
        {
            var container = await _dbContext.Containers.FindAsync(id);
            if (container == null)
            {
                return NotFound();
            }

            _dbContext.Containers.Remove(container);
            await _dbContext.SaveChangesAsync();
            return Ok("Container data deleted successfully");
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetContainersByType(string type)
        {
            if(!Enum.TryParse<ContainerType>(type, true, out var containerType))
            {
                return BadRequest("Invalid container type");
            }
            var containers = await _dbContext.Containers.Where(c => c.Type == containerType).ToListAsync();
            return Ok(containers);
        }

        [HttpGet("address/{address}")]
        public async Task<IActionResult> GetContainersByAddress(string address)
        {
            var containers = await _dbContext.Containers.Where(c => c.Address.Contains(address)).ToListAsync();
            return Ok(containers);
        }
    }
}
