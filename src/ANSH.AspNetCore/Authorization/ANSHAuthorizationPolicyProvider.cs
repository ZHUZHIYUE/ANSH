using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
namespace ANSH.AspNetCore.Authorization {

    /// <summary>
    /// 授权策略提供者
    /// </summary>
    public class ANSHAuthorizationPolicyProvider : IAuthorizationPolicyProvider {
        DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
        string _Policy => "ANSHAuthorize";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options">授权策略</param>
        public ANSHAuthorizationPolicyProvider (IOptions<AuthorizationOptions> options) {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider (options);
        }

        /// <summary>
        /// 默认策略
        /// </summary>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync () => FallbackPolicyProvider.GetDefaultPolicyAsync ();

        /// <summary>
        /// 回退策略
        /// </summary>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync () => FallbackPolicyProvider.GetFallbackPolicyAsync ();

        /// <summary>
        /// 获取策略
        /// </summary>
        /// <param name="policyName">策略名称</param>
        /// <returns></returns>
        public virtual Task<AuthorizationPolicy> GetPolicyAsync (string policyName) {
            if (policyName.StartsWith (_Policy, StringComparison.OrdinalIgnoreCase)) {
                var policy = new AuthorizationPolicyBuilder ();
                policy.AddRequirements (new ANSHRequirement ());
                return Task.FromResult (policy.Build ());
            }

            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return 
            // Task.FromResult<AuthorizationPolicy>(null) instead.
            return FallbackPolicyProvider.GetPolicyAsync (policyName);
        }
    }
}