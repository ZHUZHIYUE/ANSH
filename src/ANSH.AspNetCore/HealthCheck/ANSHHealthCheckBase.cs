using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ANSH.AspNetCore.HealthCheck {
    /// <summary>
    /// 健康检测基类
    /// </summary>
    public class ANSHHealthCheckBase : IHealthCheck {

        /// <summary>
        /// 健康检测
        /// </summary>
        /// <returns>检测结果</returns>
        public Task<HealthCheckResult> CheckHealthAsync (
            HealthCheckContext context,
            CancellationToken cancellationToken = default (CancellationToken)) {
            var healthCheckResultHealthy = true;

            if (healthCheckResultHealthy) {
                return Task.FromResult (
                    HealthCheckResult.Healthy ("A healthy result."));
            }

            return Task.FromResult (
                HealthCheckResult.Unhealthy ("An unhealthy result."));
        }
    }
}