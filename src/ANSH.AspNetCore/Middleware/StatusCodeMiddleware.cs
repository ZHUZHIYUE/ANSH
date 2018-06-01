using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ANSH.AspNetCore.Middleware
{
    /// <summary>
    /// 处理Response.StatusCode在400-599之间的中间件
    /// </summary>
    public class StatusCodeMiddleware
    {
        /// <summary>
        /// 请求委托
        /// </summary>
        readonly RequestDelegate _next;

        /// <summary>
        /// 配置处理
        /// </summary>
        readonly Action<HttpContext, Exception> _configure;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next">请求委托</param>
        /// <param name="configure">配置处理</param>
        public StatusCodeMiddleware(RequestDelegate next, Action<HttpContext, Exception> configure)
        {
            _next = next;
            _configure = configure;
        }

        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="context">当前上下文请求对象</param>
        /// <returns>返回异步操作</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            Exception _ex = null;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _ex = ex;
                context.Response.StatusCode = 500;
            }
            finally
            {
                if (context.Response.StatusCode >= 400 && context.Response.StatusCode <= 599)
                {
                    _configure(context, _ex);
                }
            }
        }
    }
}
