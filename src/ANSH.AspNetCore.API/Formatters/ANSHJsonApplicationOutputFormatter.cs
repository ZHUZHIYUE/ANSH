using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace ANSH.AspNetCore.API.Formatters {
    /// <summary>
    /// WebApi格式化响应数据，用给定的文本格式将一个物体写到输出流中
    /// </summary>
    public class ANSHJsonApplicationOutputFormatter : TextOutputFormatter {
        Action<IServiceProvider, object, Type> _Action;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="action">对响应的JSON文本格式进行读取</param>
        /// <returns></returns>
        public ANSHJsonApplicationOutputFormatter (Action<IServiceProvider, object, Type> action = null) : base () {
            _Action = action;
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
        public override async Task WriteResponseBodyAsync (OutputFormatterWriteContext context, Encoding selectedEncoding) {
            _Action?.Invoke (context.HttpContext.RequestServices, context.Object, context.ObjectType);
            await context.HttpContext.Response.WriteAsync (JObject.FromObject (context.Object).ToString (), selectedEncoding);
        }
    }
}