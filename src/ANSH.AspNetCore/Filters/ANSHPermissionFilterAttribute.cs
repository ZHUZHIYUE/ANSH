﻿using System;
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
    /// 权限授权筛选器
    /// </summary>
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ANSHPermissionFilterAttribute : Attribute, IAuthorizationFilter, IOrderedFilter {
        /// <summary>
        /// 执行顺序
        /// </summary>
        public virtual int Order => 2;

        /// <summary>
        /// 筛选器管道内最先执行的筛选器
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public virtual void OnAuthorization (AuthorizationFilterContext context) {
            if (!(context?.Filters?.Any (m => (m is IAllowAnonymousFilter)) ?? false)) {
                VerifyPermission (context);
            }
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public abstract void VerifyPermission (AuthorizationFilterContext context);
    }
}