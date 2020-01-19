using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ANSH.JWT {
    /// <summary>
    /// JWT头
    /// </summary>
    public class ANSHJWTHeader {

        /// <summary>
        /// 令牌的类型
        /// </summary>
        [JsonProperty ("typ")]
        public string Typ => "JWT";

        /// <summary>
        /// 签名使用的算法 默认为HMAC SHA256
        /// </summary>
        [JsonProperty ("alg")]
        [JsonConverter (typeof (StringEnumConverter))]
        public ANSHJWTHashAlgorithm Alg { get; set; } = ANSHJWTHashAlgorithm.HS256;
    }
}