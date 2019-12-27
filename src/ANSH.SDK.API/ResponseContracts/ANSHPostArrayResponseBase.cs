using System;
using System.Collections.Generic;
using System.Text;

namespace ANSH.SDK.API.ResponseContracts {
    /// <summary>
    /// Post响应多条记录基类
    /// </summary>
    /// <typeparam name="TANSHModelResponse">响应模型</typeparam>
    public abstract class ANSHPostArrayResponseBase<TANSHModelResponse> : ANSHResponseBase where TANSHModelResponse : Models.ANSHPostArrayResponseModelBase {

        /// <summary>
        /// 返回信息
        /// </summary>
        public virtual List<TANSHModelResponse> ResultList {
            get;
            set;
        } = new List<TANSHModelResponse> ();
    }
}