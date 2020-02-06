using System;
using System.Collections.Generic;
using System.Linq;
using ANSH.JWT;
using Newtonsoft.Json;

namespace ANSH.OAuth {

    /// <summary>
    /// 令牌值
    /// </summary>
    public class ANSHAccessToken {

        /// <summary>
        /// 授权人
        /// </summary>
        [JsonProperty ("authorize")]
        public string Authorize { get; set; }

        /// <summary>
        /// 被授权人
        /// </summary>
        [JsonProperty ("authorized")]
        public string Authorized { get; set; }

        /// <summary>
        /// 令牌类型
        /// </summary>
        [JsonProperty ("token_type")]
        public TokenTypes TokenType {
            get;
            set;
        }
    }
}