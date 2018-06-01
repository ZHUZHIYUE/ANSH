using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace ANSH.AspNetCore.Formatter {
    /// <summary>
    /// WebApi格式化响应数据，用给定的文本格式将一个物体写到输出流中
    /// </summary>
    public class JsonApplicationOutputFormatter : TextOutputFormatter {

        Action<IServiceProvider, string, Type> _action;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="action">对响应的JSON文本格式进行操作</param>
        /// <returns></returns>
        public JsonApplicationOutputFormatter (Action<IServiceProvider, string, Type> action = null) : base () {
            _action = action;
            SupportedMediaTypes.Clear ();
            SupportedMediaTypes.Add (new MediaTypeHeaderValue ("application/json"));
            SupportedEncodings.Clear ();
            SupportedEncodings.Add (Encoding.UTF8);
        }

        /// <summary>
        /// 响应
        /// </summary>
        /// <param name="context">响应上下文</param>
        /// <param name="selectedEncoding">应用于响应的编码格式</param>
        /// <returns></returns>
        public override Task WriteResponseBodyAsync (OutputFormatterWriteContext context, Encoding selectedEncoding) {
            var response_body = context.Object.ToJson ();
            _action (context.HttpContext.RequestServices, response_body, context.ObjectType);
            return context.HttpContext.Response.WriteAsync (response_body);
        }
    }
}