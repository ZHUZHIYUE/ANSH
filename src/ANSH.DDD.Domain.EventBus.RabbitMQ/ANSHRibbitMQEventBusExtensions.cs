using ANSH.DDD.Domain.EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 扩展方法
/// </summary>
public static class ANSHRibbitMQEventBusExtensions {

    /// <summary>
    /// 注册依赖注入事件总线程
    /// </summary>
    /// <param name="services">服务</param>
    /// <param name="username">帐号</param>
    /// <param name="password">密码</param>
    /// <param name="port">端口号</param>
    /// <param name="virtualhost">虚拟地址</param>
    /// <param name="hostname">主机名称</param>
    public static IServiceCollection AddANSHRibbitMQEventBus (this IServiceCollection services, string username, string password, int port, string virtualhost, string hostname) {
        services.AddScoped<IANSHRibbitMQEventBus> ((service) => {
            return new ANSHRibbitMQEventBus (username, password, port, virtualhost, hostname);
        });
        return services;
    }
}