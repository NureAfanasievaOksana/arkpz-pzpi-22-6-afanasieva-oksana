using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SortGarbageAPI.Services;

namespace SortGarbageAPI.Controllers
{
    [ApiController]
    [Route("/database")]
    public class DataBaseController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public DataBaseController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost("backup")]
        public async Task<IActionResult> BackupDatabase([FromBody] string backupPath)
        {
            await _databaseService.BackupDatabaseAsync(backupPath);
            return Ok("Database backup completed successfully");
        }

        [HttpPost("restore")]
        public async Task<IActionResult> RestoreDatabase([FromBody] string restoreFilePath)
        {
            await _databaseService.RestoreDatabaseAsync(restoreFilePath);
            return Ok("Database restore completed successfully.");
        }
    }
}