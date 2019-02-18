using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ANSH.SDK.API.ResponseContracts {
    /// <summary>
    /// 响应
    /// <para>分页</para>
    /// </summary>
    /// <typeparam name="TModelResponse">响应模型</typeparam>
    public abstract class GETByPageResponse<TModelResponse> : BaseResponse where TModelResponse : Models.GETArrayResponseModel {

        /// <summary>
        /// 返回信息
        /// </summary>
        public List<TModelResponse> result_list {
            get;
            set;
        } = new List<TModelResponse> ();

        /// <summary>
        /// 总页数
        /// </summary>
        /// <returns></returns>
        public Models.PageResponesModel page_info {
            get;
            set;
        } = new Models.PageResponesModel ();
    }
}