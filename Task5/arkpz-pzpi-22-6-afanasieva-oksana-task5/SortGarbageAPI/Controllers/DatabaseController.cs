using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SortGarbageAPI.Services;

namespace SortGarbageAPI.Controllers
{
    /// <summary>
    /// Controller responsible for database operations such as backup and restore
    /// </summary>
    [ApiController]
    [Route("/database")]
    public class DataBaseController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        /// <summary>
        /// Constructor for DatabaseController
        /// </summary>
        /// <param name="databaseService">Service for managing database operations</param>
        public DataBaseController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Creates a backup of the database
        /// </summary>
        /// <param name="backupPath">Directory path where the backup will be stored</param>
        /// <returns>Success message if backup is completed</returns>
        [HttpPost("backup")]
        public async Task<IActionResult> BackupDatabase([FromBody] string backupPath)
        {
            await _databaseService.BackupDatabaseAsync(backupPath);
            return Ok("Database backup completed successfully");
        }

        /// <summary>
        /// Restores the database from a backup file
        /// </summary>
        /// <param name="restoreFilePath">Path to the backup file</param>
        /// <returns>Success message if restore is completed</returns>
        [HttpPost("restore")]
        public async Task<IActionResult> RestoreDatabase([FromBody] string restoreFilePath)
        {
            await _databaseService.RestoreDatabaseAsync(restoreFilePath);
            return Ok("Database restore completed successfully.");
        }
    }
}