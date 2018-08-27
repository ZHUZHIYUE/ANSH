using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSH.API.RequestContracts.Models;
using ANSH.API.ResponseContracts;
using ANSH.API.ResponseContracts.Models;
using Newtonsoft.Json;
namespace ANSH.API.RequestContracts {
    /// <summary>
    /// 请求
    /// <para>数组POST</para>
    /// </summary>
    /// <typeparam name="TResponse">响应</typeparam>
    /// <typeparam name="TMODELRequest">请求模型</typeparam>
    /// <typeparam name="TModelResponse">响应模型</typeparam>
    public abstract class POSTRequest<TResponse, TMODELRequest, TModelResponse> : BaseRequest<TResponse>
        where TResponse : POSTResponse<TModelResponse>
        where TMODELRequest : POSTRequestModel
    where TModelResponse : POSTResponseModel {

        /// <summary>
        /// 数组
        /// </summary>
        public List<TMODELRequest> post_list {
            get;
            set;
        }

        /// <summary>
        /// 批量处理数组最大条数
        /// </summary>
        [JsonIgnore]
        protected abstract int post_list_numb {
            get;
        }

        /// <summary>
        /// 验证参数合法性
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <returns>验证通过返回True，验证失败返回False</returns>
        public override bool Validate (out string msg) {
            if (!base.Validate (out msg)) {
                return false;
            }
            if (!(post_list?.Count > 0)) {
                msg = $"参数post_list为空，请确保post_list有传值且满足JSON数据格式。";
                return false;
            }
            if (post_list.Count > post_list_numb) {
                msg = $"请确保参数post_list最大处理量不得超过{post_list_numb}";
                return false;
            }
            return true;
        }
    }
}