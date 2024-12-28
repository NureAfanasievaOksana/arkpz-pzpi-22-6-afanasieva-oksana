﻿using System;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace SortGarbageAPI.Services
{
    public class DatabaseService
    {
        private readonly IConfiguration _configuration;
        private readonly string _dbConnection;
        private readonly string _databaseName;
        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task BackupDatabaseAsync(string backupFolderPath)
        {
            if (string.IsNullOrEmpty(backupFolderPath) || !Directory.Exists(backupFolderPath))
            {
                throw new ArgumentException("Backup folder path is invalid or does not exist.", nameof(backupFolderPath));
            }

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupFileName = $"{databaseName}_Backup_{timestamp}.bak";
            var backupFilePath = Path.Combine(backupFolderPath, backupFileName);

            var query = $"BACKUP DATABASE [{databaseName}] TO DISK = @BackupFilePath";

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BackupFilePath", backupFilePath);
            await command.ExecuteNonQueryAsync();
        }

        public async Task RestoreDatabaseAsync(string backupFilePath)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;

            if (string.IsNullOrEmpty(backupFilePath) || !File.Exists(backupFilePath))
            {
                throw new ArgumentException("Backup file path is invalid or file does not exist.", nameof(backupFilePath));
            }

            var query = $@"USE master; ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE [{databaseName}] FROM DISK = @BackupFilePath;
                ALTER DATABASE [{databaseName}] SET MULTI_USER;";

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BackupFilePath", backupFilePath);
            await command.ExecuteNonQueryAsync();
        }
    }
}