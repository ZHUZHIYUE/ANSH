using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ANSH.AspNetCore.Filters.API {

    /// <summary>
    /// 验证MediaType
    /// </summary>
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class MediaTypeFilterAttribute : Attribute, IActionFilter, IOrderedFilter {
        /// <summary>
        /// 执行顺序
        /// </summary>
        public virtual int Order => 1;

        /// <summary>
        /// Action执行前
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public virtual void OnActionExecuting (ActionExecutingContext context) {
            if (context.HttpContext.Request.Method.ToLower () != "POST".ToLower () || context.HttpContext.Request.Method.ToLower () == "POST".ToLower () &&
                MediaTypeHeaderValue.TryParse (context.HttpContext.Request.ContentType, out MediaTypeHeaderValue mediatype) &&
                MediaTypeWhiteList.Exists (m => m.MediaType.ToLower () == mediatype.MediaType.ToLower () && m.CharSet.ToLower () == mediatype.CharSet.ToLower ())) {
                MediaTypeStatePass (context);
            } else {
                MediaTypeStateFail (context);
            }
        }

        /// <summary>
        /// Action执行后
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public virtual void OnActionExecuted (ActionExecutedContext context) {

        }
        /// <summary>
        /// 支持的MediaType
        /// </summary>
        public abstract List<MediaTypeHeaderValue> MediaTypeWhiteList {
            get;
        }

        /// <summary>
        /// MediaType 验证通过
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public abstract void MediaTypeStatePass (ActionExecutingContext context);

        /// <summary>
        /// MediaType 验证未通过
        /// </summary>
        /// <param name="context">当前请求上下文</param>
        public abstract void MediaTypeStateFail (ActionExecutingContext context);
    }
}