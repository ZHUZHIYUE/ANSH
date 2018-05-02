using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSH.OAuth
{
    /// <summary>
    /// 授权码
    /// </summary>
    public class TokenCode
    {
        /// <summary>
        /// 授权码值
        /// </summary>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 授权码值有效时间（单位分）
        /// </summary>
        public int Expires_IN
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
    }
}
