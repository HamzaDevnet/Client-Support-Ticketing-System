using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;
using CSTS.DAL;
using CSTS.DAL.Repository.IRepository;

namespace CSTS.API.Health
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly IUnitOfWork _unitOfWork;

        public DatabaseHealthCheck(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await _unitOfWork.CanConnectAsync();
                return canConnect
                    ? HealthCheckResult.Healthy("Database connection is healthy.")
                    : HealthCheckResult.Unhealthy("Database connection is unhealthy.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database connection check failed.", ex);
            }
        }
    }
}
