using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ANSH.AspNetCore.Filters.MVC {

    /// <summary>
    /// 操作筛选器
    /// </summary>
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ModelStateFilterAttribute : Attribute, IActionFilter, IOrderedFilter {
        /// <summary>
        /// 执行顺序
        /// </summary>
        public virtual int Order => 1;

        /// <summary>
        /// Action执行前
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public virtual void OnActionExecuting (ActionExecutingContext context) {

            if (context.HttpContext.Request.Method.ToLower () == "POST".ToLower ()) {
                if (context.ModelState.IsValid) {
                    ModelStatePass (context);
                } else {
                    ModelStateFail (context);
                }
            }
        }

        /// <summary>
        /// Action执行后
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public virtual void OnActionExecuted (ActionExecutedContext context) {

        }

        /// <summary>
        /// ModelState 验证通过
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public abstract void ModelStatePass (ActionExecutingContext context);

        /// <summary>
        /// ModelState 验证未通过
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public abstract void ModelStateFail (ActionExecutingContext context);
    }
}