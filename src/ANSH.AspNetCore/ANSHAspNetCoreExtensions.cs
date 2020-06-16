using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANSH.AspNetCore.Authorization;
using ANSH.AspNetCore.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>
/// 拓展类方法
/// </summary>
public static class ANSHAspNetCoreExtensions {
    /// <summary>
    /// 获取完整URL地址
    /// </summary>
    /// <param name="context">当前上下文请求信息</param>
    /// <returns>完整URL地址</returns>
    public static string GetFullURL (this HttpContext context) => $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path.Value}{context.Request.QueryString.Value}";

    /// <summary>
    /// 获取客户端IP地址
    /// </summary>
    /// <param name="context">当前上下文请求信息</param>
    /// <returns>客户端IP地址</returns>
    public static string GetClientIP (this HttpContext context) {
        string ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault ();
        return string.IsNullOrWhiteSpace (ip) ? context.Connection.RemoteIpAddress.ToString () : ip.Split (new string[] { "," }, StringSplitOptions.RemoveEmptyEntries) [0];
    }

    /// <summary>
    /// 添加一个处理Response.StatusCode在400-599之间的中间件
    /// </summary>
    /// <param name="builder">当前对象</param>
    /// <param name="configure">配置处理</param>
    /// <returns>当前对象</returns>
    public static IApplicationBuilder UseStatusCode (this IApplicationBuilder builder, Action<HttpContext, Exception> configure) => builder.UseMiddleware<StatusCodeMiddleware> (configure);

    /// <summary>
    /// 使用Redis保存Keys
    /// <remark></remark>
    /// </summary>
    /// <param name="builder">当前对象</param>
    /// <param name="cachekey">redis key</param>
    /// <returns>返回IDataProtectionBuilder</returns>
    public static IDataProtectionBuilder PersistKeysToIDistributedCache (this IDataProtectionBuilder builder, string cachekey = "PersistKeysToRedisIXmlRepository") {
        if (builder == null) {
            throw new ArgumentNullException (nameof (builder));
        }

        builder.SetApplicationName (cachekey);
        var descriptor = ServiceDescriptor.Singleton<IXmlRepository> (services => new RedisXmlRepository (services.GetService<IDistributedCache> (), cachekey));
        builder.Services.Add (descriptor);
        return builder.AddKeyManagementOptions (optinos => optinos.XmlRepository = builder.Services.BuildServiceProvider ().GetService<IXmlRepository> ());
    }

    /// <summary>
    /// 添加身份验证策略
    /// <remark></remark>
    /// </summary>
    public static void AddANSHAuthorization (this IServiceCollection services, Action<IServiceCollection> action) {
        services.AddSingleton<IAuthorizationPolicyProvider, ANSHAuthorizationPolicyProvider> ();
        action (services);
    }

    /// <summary>
    /// 添加后台托管任务
    /// </summary>
    /// <typeparam name="THostedService">后台托管任务</typeparam>
    /// <param name="services"></param>
    /// <param name="replicas">复制多少份</param>
    public static void AddHostedService<THostedService> (this IServiceCollection services, int replicas) where THostedService : class, IHostedService {
        for (int i = 0; i < replicas; i++) {
            services.AddSingleton<IHostedService, THostedService> ();
        }
    }

    /// <summary>
    /// 添加身份验证策略
    /// <remark></remark>
    /// </summary>
    public static void AddANSHAuthorization<TIAuthorizationPolicyProvider> (this IServiceCollection services, Action<IServiceCollection> action)
    where TIAuthorizationPolicyProvider : class, IAuthorizationPolicyProvider {
        services.AddSingleton<IAuthorizationPolicyProvider, TIAuthorizationPolicyProvider> ();
        action (services);
    }
}