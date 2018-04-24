using System;
using System.Collections.Generic;
using System.Linq;

namespace ANSH.Common.OAuth
{

    /// <summary>
    /// 令牌
    /// </summary>
    public class Token
    {

        /// <summary>
        /// 令牌值
        /// </summary>
        public string Access_Token
        {
            get; set;
        }

        /// <summary>
        /// 令牌值有效时间（单位分）
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

        /// <summary>
        /// 刷新令牌值
        /// </summary>
        public string Refresh_Token
        {
            get;
            set;
        }

        /// <summary>
        /// 刷新令牌有效时间（单位分）
        /// </summary>
        public int Refexpires_IN
        {
            get;
            set;
        }
    }
}