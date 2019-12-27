using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ANSH.SDK.API.ResponseContracts.Models;

namespace ANSH.SDK.API.ResponseContracts {
    /// <summary>
    /// Get分页响应多条记录基类
    /// </summary>
    /// <typeparam name="TANSHModelResponse">响应模型</typeparam>
    /// <typeparam name="TANSHPageResponesModel">分页信息模型</typeparam>
    public abstract class ANSHGetByPageResponseBase<TANSHModelResponse, TANSHPageResponesModel> : ANSHGetListResponseBase<TANSHModelResponse> where TANSHModelResponse : class
    where TANSHPageResponesModel : IANSHPageResponesModelBase, new () {

        /// <summary>
        /// 总页数
        /// </summary>
        /// <returns></returns>
        public virtual TANSHPageResponesModel PageInfo { get; set; } = new TANSHPageResponesModel ();
    }
}