using System;
using System.Collections.Generic;
using System.Text;

namespace ANSH.SDK.API.ResponseContracts {
    /// <summary>
    /// 响应
    /// <para>数组POST</para>
    /// </summary>
    /// <typeparam name="TModelResponse">响应模型</typeparam>
    public abstract class POSTArrayResponse<TModelResponse> : BaseResponse where TModelResponse : Models.POSTArrayResponseModel {

        /// <summary>
        /// 返回信息
        /// </summary>
        public List<TModelResponse> result_list {
            get;
            set;
        } = new List<TModelResponse> ();
    }
}