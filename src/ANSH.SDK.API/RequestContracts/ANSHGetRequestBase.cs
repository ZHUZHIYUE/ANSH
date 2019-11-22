using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSH.SDK.API.RequestContracts.Models;
using ANSH.SDK.API.ResponseContracts;

namespace ANSH.SDK.API.RequestContracts {

    /// <summary>
    /// Get请求单条记录基类
    /// </summary>
    /// <typeparam name="TANSHQueryRequest">Get请求查询模型基类</typeparam>
    /// <typeparam name="TANSHGetResponse">Get响应单条记录基类</typeparam>
    /// <typeparam name="TANSHModelResponse">响应模型基类</typeparam>
    public abstract class ANSHGetRequestBase<TANSHQueryRequest, TANSHGetResponse, TANSHModelResponse> : ANSHRequestBase<TANSHGetResponse>
        where TANSHGetResponse : ANSHGetResponseBase<TANSHModelResponse>
        where TANSHModelResponse : class
    where TANSHQueryRequest : ANSHGetRequestModelBase {

        /// <summary>
        /// 查询模型
        /// </summary>
        /// <value></value>
        public TANSHQueryRequest Query { get; set; }
    }
}