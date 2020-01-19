using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSH.OAuth {
    /// <summary>
    /// 令牌类型
    /// </summary>
    public enum TokenTypes {
        /// <summary>
        /// 授权码code
        /// </summary>
        code,
        /// <summary>
        /// 通过code或reftoken创建token
        /// </summary>
        token,
        /// <summary>
        /// code_token对应的reftoken
        /// </summary>
        reftoken,
        /// <summary>
        /// 通过应用创建token
        /// </summary>
        client_token,
    }
}