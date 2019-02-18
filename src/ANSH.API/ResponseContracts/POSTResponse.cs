using System;
using System.Collections.Generic;
using System.Text;

namespace ANSH.API.ResponseContracts {
    /// <summary>
    /// 响应
    /// </summary>
    /// <typeparam name="TModelResponse">响应模型</typeparam>
    public abstract class POSTResponse<TModelResponse> : BaseResponse where TModelResponse : class {

        /// <summary>
        /// 返回信息
        /// </summary>
        public TModelResponse result_item {
            get;
            set;
        }
    }
}