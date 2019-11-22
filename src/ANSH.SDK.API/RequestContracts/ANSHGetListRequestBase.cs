using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSH.SDK.API.RequestContracts.Models;
using ANSH.SDK.API.ResponseContracts;
using ANSH.SDK.API.ResponseContracts.Models;

namespace ANSH.SDK.API.RequestContracts {
    /// <summary>
    /// 请求
    /// </summary>
    /// <typeparam name="TANSHQueryRequest">查询参数</typeparam>
    /// <typeparam name="TANSHResponse">响应</typeparam>
    /// <typeparam name="TANSHModelResponse">响应模型</typeparam>
    public abstract class ANSHGetListRequestBase<TANSHQueryRequest, TANSHResponse, TANSHModelResponse> : ANSHRequestBase<TANSHResponse>
        where TANSHResponse : ANSHGetListResponseBase<TANSHModelResponse>
        where TANSHModelResponse : class
    where TANSHQueryRequest : ANSHGetRequestModelBase {

        /// <summary>
        /// 查询参数
        /// </summary>
        /// <value></value>
        public TANSHQueryRequest Query { get; set; }
    }
}