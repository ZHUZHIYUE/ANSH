using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSH.OAuth
{  /// <summary>
   /// 令牌信息
   /// </summary>
    public class StoreToken
    {
        /// <summary>
        /// 令牌类型
        /// </summary>
        public enum TokenTypes
        {
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

        /// <summary>
        /// 授权人
        /// </summary>
        public string Authorize
        {
            get;
            set;
        }

        /// <summary>
        /// 被授权人
        /// <para>应用ID appid</para>
        /// </summary>
        public string Authorized
        {
            get;
            set;
        }

        /// <summary>
        /// 令牌值
        /// </summary>
        public string Token
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create_Times
        {
            get;
            set;
        }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expires_Times
        {
            get;
            set;

        }

        /// <summary>
        /// 令牌类型
        /// </summary>
        public TokenTypes Token_Type
        {
            get;
            set;
        }
    }
}
