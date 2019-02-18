using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSH.OAuth {
    /// <summary>
    /// 授权方式
    /// </summary>
    public enum GrantTypes {
        /// <summary>
        /// 授权码模式创建令牌
        /// </summary>
        authorization_code,

        /// <summary>
        /// 刷新凭证创建令牌
        /// </summary>
        refresh_token,

        /// <summary>
        /// 客户端模式创建令牌
        /// </summary>
        client_credentials
    }
}