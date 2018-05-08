using System;
using System.Collections.Generic;
using System.Linq;
using ANSH.API.ResponseContracts;
using Newtonsoft.Json;

namespace ANSH.API.RequestContracts {
    /// <summary>
    /// 请求
    /// </summary>
    public abstract class BaseRequest {

        /// <summary>
        /// 验证时间合法性
        /// </summary>
        /// <param name="time_start">起始时间</param>
        /// <param name="time_end">结束时间</param>
        /// <param name="time_type">时间格式</param>
        /// <returns>验证通过返回True，验证失败返回False</returns>
        protected bool ValidateDateTime (string time_start, string time_end, string time_type = "yyyy-MM-dd HH:mm:ss") {
            time_start = time_start ?? string.Empty;
            time_end = time_end ?? string.Empty;
            if (!time_start.IsDateTime (out DateTime? time_starts, time_type) ||
                !time_end.IsDateTime (out DateTime? time_ends, time_type) ||
                DateTime.Compare (time_starts.Value, time_ends.Value) > 0) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// API方法名称
        /// </summary>
        [JsonProperty]
        public abstract string APIName {
            get;
        }

        /// <summary>
        /// API版本号
        /// </summary>
        [JsonProperty]
        public abstract string APIVersion {
            get;
        }
    }
}