using System;
using System.Collections.Generic;
using System.Text;

namespace ANSH.API.ResponseContracts {
    /// <summary>
    /// 响应
    /// <para>数组POST</para>
    /// </summary>
    /// <typeparam name="TModelResponse">响应模型</typeparam>
    public abstract class POSTResponse<TModelResponse> : BaseResponse where TModelResponse : Models.POSTResponseModel {

        /// <summary>
        /// 返回信息
        /// </summary>
        public List<TModelResponse> result_list {
            get;
            set;
        } = new List<TModelResponse> ();
    }
}