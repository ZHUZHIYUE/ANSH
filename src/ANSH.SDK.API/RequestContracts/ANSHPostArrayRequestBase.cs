using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSH.SDK.API.RequestContracts.Models;
using ANSH.SDK.API.ResponseContracts;
using ANSH.SDK.API.ResponseContracts.Models;
using Newtonsoft.Json;
namespace ANSH.SDK.API.RequestContracts {
    /// <summary>
    /// Post请求多条记录基类
    /// </summary>
    /// <typeparam name="TANSHResponse">Post响应多条记录基类</typeparam>
    /// <typeparam name="TANSHMODELRequest">Post请求多条模型</typeparam>
    /// <typeparam name="TANSHModelResponse">响应模型</typeparam>
    public abstract class ANSHPostArrayRequestBase<TANSHResponse, TANSHMODELRequest, TANSHModelResponse> : ANSHRequestBase<TANSHResponse>
        where TANSHResponse : ANSHPostArrayResponseBase<TANSHModelResponse>
        where TANSHMODELRequest : ANSHPostRequestModelBase
    where TANSHModelResponse : ANSHPostArrayResponseModelBase {

        /// <summary>
        /// 数组
        /// </summary>
        public virtual List<TANSHMODELRequest> PostList {
            get;
            set;
        }

        /// <summary>
        /// 批量处理数组最大条数
        /// </summary>
        [JsonIgnore]
        protected abstract int PostListNumb {
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
            if (!(PostList?.Count > 0)) {
                msg = $"参数postList为空，请确保postList有传值且满足JSON数据格式。";
                return false;
            }
            if (PostList.Count > PostListNumb) {
                msg = $"请确保参数postList最大处理量不得超过{PostListNumb}";
                return false;
            }
            return true;
        }
    }
}