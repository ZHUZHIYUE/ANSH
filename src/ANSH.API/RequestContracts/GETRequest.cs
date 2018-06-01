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
    public abstract class GETRequest<TResponse> : BaseRequest where TResponse : BaseResponse {

        /// <summary>
        /// 获取URL参数
        /// </summary>
        public virtual Dictionary<string, string> GetParameters () {
            return SetParameters();
        }

        /// <summary>
        /// 设置URL参数
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, string> SetParameters ();
    }
}