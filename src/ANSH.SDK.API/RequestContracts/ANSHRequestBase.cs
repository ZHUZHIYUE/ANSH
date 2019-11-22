using System;
using System.Collections.Generic;
using System.Linq;
using ANSH.SDK.API.ResponseContracts;
using Newtonsoft.Json;

namespace ANSH.SDK.API.RequestContracts {
    /// <summary>
    /// 请求基类
    /// </summary>
    /// <typeparam name="TANSHResponseBase">响应基类</typeparam>
    [JsonObject (MemberSerialization.OptOut)]
    public abstract class ANSHRequestBase<TANSHResponseBase>
        where TANSHResponseBase : ANSHResponseBase {
            /// <summary>
            /// 验证参数合法性
            /// </summary>
            /// <param name="msg">提示信息</param>
            /// <returns>验证通过返回True，验证失败返回False</returns>
            public virtual bool Validate (out string msg) {
                msg = "SUCCESS";
                return true;
            }

            /// <summary>
            /// API方法名称
            /// </summary>
            [JsonIgnore]
            public abstract string APIName {
                get;
            }

            /// <summary>
            /// API版本号
            /// </summary>
            [JsonIgnore]
            public abstract string APIVersion {
                get;
            }
        }
}