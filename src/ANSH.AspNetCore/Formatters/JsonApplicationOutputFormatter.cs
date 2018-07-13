using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace ANSH.AspNetCore.Formatters {
    /// <summary>
    /// WebApi格式化响应数据，用给定的文本格式将一个物体写到输出流中
    /// </summary>
    public class JsonApplicationOutputFormatter : TextOutputFormatter {

        Func<IServiceProvider, object, Type, JObject> _func;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="func">对响应的JSON文本格式进行操作</param>
        /// <returns></returns>
        public JsonApplicationOutputFormatter (Func<IServiceProvider, object, Type, JObject> func = null) : base () {
            _func = func;
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
        public override Task WriteResponseBodyAsync (OutputFormatterWriteContext context, Encoding selectedEncoding) {

            var jobject =
                _func != null ?
                _func.Invoke (context.HttpContext.RequestServices, context.Object, context.ObjectType) :
                JObject.FromObject (context.Object);
            return context.HttpContext.Response.WriteAsync (jobject.ToString (), selectedEncoding);
        }
    }
}