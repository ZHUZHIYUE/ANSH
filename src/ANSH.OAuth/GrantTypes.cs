using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSH.OAuth
{
    /// <summary>
    /// 授权方式
    /// </summary>
    public enum GrantTypes
    {
        /// <summary>
        /// 通过授权码创建令牌
        /// </summary>
        authorization_code,

        /// <summary>
        /// 通过刷新凭证创建令牌
        /// </summary>
        refresh_token
    }
}
