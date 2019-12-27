using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSH.SDK.API.RequestContracts.Models;
using ANSH.SDK.API.ResponseContracts;
using ANSH.SDK.API.ResponseContracts.Models;

namespace ANSH.SDK.API.RequestContracts {
    /// <summary>
    /// Get分页请求多条记录基类
    /// </summary>
    /// <typeparam name="TANSHQueryRequest">查询参数</typeparam>
    /// <typeparam name="TANSHResponse">响应</typeparam>
    /// <typeparam name="TANSHModelResponse">响应模型</typeparam>
    /// <typeparam name="TANSHPageResponesModel">分页信息模型</typeparam>
    public abstract class ANSHGetByPageRequestBase<TANSHQueryRequest, TANSHResponse, TANSHModelResponse, TANSHPageResponesModel> : ANSHGetListRequestBase<TANSHQueryRequest, TANSHResponse, TANSHModelResponse>
        where TANSHResponse : ANSHGetByPageResponseBase<TANSHModelResponse, TANSHPageResponesModel>
        where TANSHModelResponse : class
    where TANSHPageResponesModel : IANSHPageResponesModelBase, new ()
    where TANSHQueryRequest : ANSHGetByPageRequestModelBase {

    }
}