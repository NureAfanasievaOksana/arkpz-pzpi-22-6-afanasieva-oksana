using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Services
{
    /// <summary>
    /// Service for managing container operations
    /// </summary>
    public class ContainerService
    {
        private readonly SortGarbageDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the ContainerService
        /// </summary>
        /// <param name="dbContext">Database context for container operations</param>
        public ContainerService(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all containers from the database
        /// </summary>
        /// <returns>List of all containers</returns>
        public async Task<List<Container>> GetAllContainersAsync()
        {
            return await _dbContext.Containers.ToListAsync();
        }

        /// <summary>
        /// Creates a new container in the database
        /// </summary>
        /// <param name="container">The container to create</param>
        /// <returns>The created container with assigned id</returns>
        public async Task<Container> CreateContainerAsync(Container container)
        {
            _dbContext.Containers.Add(container);
            await _dbContext.SaveChangesAsync();
            return container;
        }

        /// <summary>
        /// Retrieves a specific container by its id
        /// </summary>
        /// <param name="id">The container id</param>
        /// <returns>The container if found, null otherwise</returns>
        public async Task<Container?> GetContainerByIdAsync(int id)
        {
            return await _dbContext.Containers.FindAsync(id);
        }

        /// <summary>
        /// Updates an existing container in the database
        /// </summary>
        /// <param name="id">The id of the container to update</param>
        /// <param name="updatedData">The updated container data</param>
        /// <returns>True if update was successful, false if container wasn't found</returns>
        public async Task<bool> UpdateContainerAsync(int id, Container updatedData)
        {
            var container = await _dbContext.Containers.FindAsync(id);
            if (container == null)
            {
                return false;
            }

            container.Address = updatedData.Address;
            container.Type = updatedData.Type;
            container.MaxSize = updatedData.MaxSize;
            container.UserId = updatedData.UserId;
            _dbContext.Containers.Update(container);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a container from the database
        /// </summary>
        /// <param name="id">The id of the container to delete</param>
        /// <returns>True if deletion was successful, false if container wasn't found</returns>
        public async Task<bool> DeleteContainerAsync(int id)
        {
            var container = await _dbContext.Containers.FindAsync(id);
            if (container == null)
            {
                return false;
            }

            _dbContext.Containers.Remove(container);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves all containers of a specific type
        /// </summary>
        /// <param name="type">The type of containers to retrieve</param>
        /// <returns>Collection of containers of the specified type</returns>
        public async Task<IEnumerable<Container>> GetContainersByTypeAsync(ContainerType type)
        {
            return await _dbContext.Containers.Where(c => c.Type == type).ToListAsync();
        }

        /// <summary>
        /// Retrieves all containers at a specific address or containing the address string
        /// </summary>
        /// <param name="address">The address to search for</param>
        /// <returns>Collection of containers matching the address criteria</returns>
        public async Task<IEnumerable<Container>> GetContainersByAddressAsync(string address)
        {
            return await _dbContext.Containers.Where(c => c.Address.Contains(address)).ToListAsync();
        }
    }
}