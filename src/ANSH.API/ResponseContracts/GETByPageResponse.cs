using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ANSH.API.ResponseContracts {
    /// <summary>
    /// 响应
    /// <para>分页</para>
    /// </summary>
    /// <typeparam name="TModelResponse">响应模型</typeparam>
    public abstract class GETByPageResponse<TModelResponse> : BaseResponse where TModelResponse : Model.GETArrayResponseModel {

        /// <summary>
        /// 返回信息
        /// </summary>
        public List<TModelResponse> result_list {
            get;
            set;
        }

        /// <summary>
        /// 总页数
        /// </summary>
        /// <returns></returns>
        public Model.PageResponesModel page_info {
            get;
            set;
        }
    }
}