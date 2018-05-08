using System;
using System.Linq;
using ANSH.AspNetCore.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
        return string.IsNullOrWhiteSpace (ip) ? context.Connection.RemoteIpAddress.ToString () : ip;
    }

    /// <summary>
    /// 添加一个处理Response.StatusCode在400-599之间的中间件
    /// </summary>
    /// <param name="builder">当前对象</param>
    /// <param name="configure">配置处理</param>
    /// <returns>当前对象</returns>
    public static IApplicationBuilder UseStatusCode (this IApplicationBuilder builder, Action<HttpContext, Exception> configure) {
        return builder.UseMiddleware<StatusCodeMiddleware> (configure);
    }
}