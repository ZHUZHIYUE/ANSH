using Microsoft.AspNetCore.Authorization;

namespace ANSH.AspNetCore.Authorization {

    /// <summary>
    /// 授权属性
    /// </summary>
    public class ANSHAuthorizeAttribute : AuthorizeAttribute {
        string _Policy => "ANSHAuthorize";

        /// <summary>
        /// 构造函数
        /// </summary>
        public ANSHAuthorizeAttribute () {
            base.Policy = _Policy;
        }
    }
}