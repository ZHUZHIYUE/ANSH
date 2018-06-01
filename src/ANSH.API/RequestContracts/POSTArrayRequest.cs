using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSH.API.RequestContracts.Model;
using ANSH.API.ResponseContracts;
using ANSH.API.ResponseContracts.Model;
using Newtonsoft.Json;
namespace ANSH.API.RequestContracts {
    /// <summary>
    /// 请求
    /// <para>数组POST</para>
    /// </summary>
    /// <typeparam name="TResponse">响应</typeparam>
    /// <typeparam name="TMODELRequest">请求模型</typeparam>
    /// <typeparam name="TModelResponse">响应模型</typeparam>
    public abstract class POSTArrayRequest<TResponse, TMODELRequest, TModelResponse> : POSTRequest<TResponse>
        where TResponse : POSTArrayResponse<TModelResponse>
        where TMODELRequest : POSTArrayRequestModel
    where TModelResponse : POSTArrayResponseModel {

        /// <summary>
        /// 数组
        /// </summary>
        protected List<TMODELRequest> post_list {
            get;
            set;
        }

        /// <summary>
        /// 批量处理数组最大条数
        /// </summary>
        [JsonProperty]
        protected abstract int post_list_numb {
            get;
        }
    }
}