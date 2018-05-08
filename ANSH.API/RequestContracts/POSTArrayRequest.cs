using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSH.API.ResponseContracts;
using Newtonsoft.Json;
namespace ANSH.API.RequestContracts {
    /// <summary>
    /// 请求
    /// <para>数组POST</para>
    /// </summary>
    /// <typeparam name="TResponse">响应</typeparam>
    /// <typeparam name="TMODELRequest">请求模型</typeparam>
    /// <typeparam name="TModelResponse">响应模型</typeparam>
    public abstract class POSTRequest<TResponse, TMODELRequest, TModelResponse> : BaseRequest
    where TResponse : POSTArrayResponse<TModelResponse>
        where TMODELRequest : POSTRequest<TResponse, TMODELRequest, TModelResponse>.POSTArrayModelRequest
    where TModelResponse : POSTArrayResponse<TModelResponse>.POSTArrayModelResponse {

        /// <summary>
        /// 数组
        /// </summary>
        protected List<TMODELRequest> array_list {
            get;
            set;
        }

        /// <summary>
        /// 批量处理数组最大条数
        /// </summary>
        protected virtual int _array_numb {
            get;
        } = 100;

        /// <summary>
        /// 数组对象
        /// </summary>
        public class POSTArrayModelRequest {

        }
    }
}