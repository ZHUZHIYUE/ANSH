using System;
using ANSH.SDK.API.ResponseContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ANSH.AspNetCore.API {
    /// <summary>
    /// APIController基类
    /// </summary>
    [ApiController]
    [Route ("[controller]/[action]")]
    public class ANSHControllerBase : ControllerBase {

        /// <summary>
        /// 日志记录
        /// </summary>
        protected ILogger Logger {
            get;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Logger">日志记录</param>
        public ANSHControllerBase (ILogger Logger) {
            this.Logger = Logger;
        }
    }
}