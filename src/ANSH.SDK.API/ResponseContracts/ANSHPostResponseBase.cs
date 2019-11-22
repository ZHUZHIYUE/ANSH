﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ANSH.SDK.API.ResponseContracts {
    /// <summary>
    /// 响应
    /// </summary>
    /// <typeparam name="TANSHModelResponse">响应模型</typeparam>
    public abstract class ANSHPostResponseBase<TANSHModelResponse> : ANSHResponseBase where TANSHModelResponse : class {

        /// <summary>
        /// 返回信息
        /// </summary>
        public virtual TANSHModelResponse ResultItem {
            get;
            set;
        }
    }
}