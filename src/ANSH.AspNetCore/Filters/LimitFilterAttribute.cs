using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;

namespace ANSH.AspNetCore.Filters {
    /// <summary>
    /// 限制筛选器
    /// </summary>
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class LimitFilterAttribute : Attribute, IAuthorizationFilter, IOrderedFilter {
        /// <summary>
        /// 执行顺序
        /// </summary>
        public virtual int Order => 3;

        /// <summary>
        /// 筛选器管道内最先执行的筛选器
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public abstract void OnAuthorization (AuthorizationFilterContext context);
    }
}