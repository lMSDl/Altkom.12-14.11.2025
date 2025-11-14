using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks
{
    public class DirectoryAccessHealth : IHealthCheck
    {

        public string DirectoryPath { get; set; }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if(Directory.Exists(DirectoryPath))
                {
                    return Task.FromResult(HealthCheckResult.Healthy($"The directory '{DirectoryPath}' exists and is accessible."));
                }
                else
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy($"The directory '{DirectoryPath}' does not exist or is not accessible."));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Degraded($"An error occurred while checking the directory '{DirectoryPath}'", ex));
            }
        }
    }
}
