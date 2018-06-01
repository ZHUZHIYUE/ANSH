using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace ANSH.AspNetCore.Formatter {
    /// <summary>
    /// WebApi自定义格式化程序，从请求体中读取一个带有JSON文本格式的物体。
    /// </summary>
    public class JsonApplicationInputFormatter : TextInputFormatter {

        Action<IServiceProvider, string, Type> _action;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="action">对读取的JSON文本格式进行操作</param>
        /// <returns></returns>
        public JsonApplicationInputFormatter (Action<IServiceProvider, string, Type> action = null) : base () {
            _action = action;
            SupportedMediaTypes.Clear ();
            SupportedMediaTypes.Add (new MediaTypeHeaderValue ("application/json"));
            SupportedEncodings.Clear ();
            SupportedEncodings.Add (Encoding.UTF8);
        }

        /// <summary>
        /// 从请求体中读取一个对象
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>完成对请求体进行反序列化的任务</returns>
        public override Task<InputFormatterResult> ReadRequestBodyAsync (InputFormatterContext context, Encoding encoding) {
            try {
                if (context == null) {
                    throw new ArgumentNullException (nameof (context));
                }

                var request = context.HttpContext.Request;

                using (var reader = new StreamReader (request.Body, encoding)) {
                    string body_str = reader.ReadToEnd ();

                    _action (context.HttpContext.RequestServices, body_str, context.ModelType);

                    var request_body = body_str.ToJsonObj (context.ModelType);

                    return InputFormatterResult.SuccessAsync (request_body);
                }
            } catch {
                return InputFormatterResult.FailureAsync ();
            }
        }
    }
}