using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ANSH.SDK.API.ResponseContracts {
    /// <summary>
    /// Get响应单条记录基类
    /// </summary>
    /// <typeparam name="TANSHModelResponse">响应模型基类</typeparam>
    public abstract class ANSHGetResponseBase<TANSHModelResponse> : ANSHResponseBase where TANSHModelResponse : class {

        /// <summary>
        /// 响应模型
        /// </summary>
        public virtual TANSHModelResponse ResultItem { get; set; }
    }
}