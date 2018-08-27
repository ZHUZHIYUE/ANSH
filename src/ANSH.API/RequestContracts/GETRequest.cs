using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSH.API.ResponseContracts;

namespace ANSH.API.RequestContracts {

    /// <summary>
    /// 请求
    /// <para>GET</para>
    /// </summary>
    /// <typeparam name="TResponse">响应</typeparam>
    public abstract class GETRequest<TResponse> : BaseRequest<TResponse> where TResponse : BaseResponse {

        /// <summary>
        /// 获取URL参数
        /// </summary>
        /// <returns>url参数集合</returns>
        public virtual Dictionary<string, string> GetParameters () {
            return SetParameters ();
        }

        /// <summary>
        /// 设置URL参数
        /// </summary>
        /// <returns>url参数集合</returns>
        public abstract Dictionary<string, string> SetParameters ();
    }
}