using ANSH.Caches.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

/// <summary>
/// 扩展方法
/// </summary>
public static class ANSHCachesRedisExtensions {
    /// <summary>
    /// 注册依赖注入Redis
    /// </summary>
    /// <param name="services">服务</param>
    /// <param name="connectString">redis链接地址</param>
    public static IServiceCollection AddANSHRedisCache (this IServiceCollection services, string connectString) {
        services.AddSingleton<ConnectionMultiplexer> ((service) => {
            return ConnectionMultiplexer.Connect (connectString);
        });
        services.AddSingleton<ANSHCachesRedisHandle> ();
        return services;
    }
}