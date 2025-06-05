using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Truhome.Domain.Constraints;

namespace Truhome.Domain.Services
{
    public class DatabaseSetupService
    {
        private readonly ILogger<DatabaseSetupService> _logger;
        private readonly IConfiguration _configuration;

        public DatabaseSetupService(ILogger<DatabaseSetupService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task CreateSchemaFromSqlFileAsync()
        {
            try
            {
                using NpgsqlConnection connection = new NpgsqlConnection(_configuration["DbConnectionString"]!);

                await connection.OpenAsync().ConfigureAwait(false);

                await connection.ExecuteAsync(Sql.SCHEMA_SCRIPT).ConfigureAwait(false);

                _logger.LogInformation("Database schema is created or updated successfully.");
            }
            catch (PostgresException ex)
            {
                _logger.LogError(ex, "{message}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{message}", ex.Message);
            }
        }
    }
}
