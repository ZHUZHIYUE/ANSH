using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSH.OAuth
{

    /// <summary>
    /// 刷新令牌
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// 刷新令牌值
        /// </summary>
        public string Refresh_Token
        {
            get;
            set;
        }

        /// <summary>
        /// 刷新令牌值有效时间（单位分）
        /// </summary>
        public int Expires_IN { get; set; }


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
