using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Services
{
    public class ContainerService
    {
        private readonly SortGarbageDbContext _dbContext;

        public ContainerService(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Container>> GetAllContainersAsync()
        {
            return await _dbContext.Containers.ToListAsync();
        }

        public async Task<Container?> GetContainerByIdAsync(int id)
        {
            return await _dbContext.Containers.FindAsync(id);
        }

        public async Task<Container> CreateContainerAsync(Container container)
        {
            _dbContext.Containers.Add(container);
            await _dbContext.SaveChangesAsync();
            return container;
        }

        public async Task<bool> UpdateContainerAsync(int id, Container updatedData)
        {
            var container = await _dbContext.Containers.FindAsync(id);
            if (container == null) return false;

            container.Address = updatedData.Address;
            container.Type = updatedData.Type;
            container.MaxSize = updatedData.MaxSize;
            _dbContext.Containers.Update(container);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContainerAsync(int id)
        {
            var container = await _dbContext.Containers.FindAsync(id);
            if (container == null) return false;

            _dbContext.Containers.Remove(container);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
