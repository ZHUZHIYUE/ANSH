using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ANSH.AspNetCore.Mvc.Filter {

    /// <summary>
    /// 异常处理
    /// <para>异常筛选器</para>
    /// </summary>
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ExceptionFilterAttribute : Attribute, IExceptionFilter {
        /// <summary>
        /// 用于处理控制器创建、模型绑定、操作筛选器或操作方法中发生的未经处理的异常
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public void OnException (ExceptionContext context) {
            context.ExceptionHandled = !OnException (context.Exception);
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <returns>是否发送异常响应</returns>
        public abstract bool OnException (Exception ex);

    }
}