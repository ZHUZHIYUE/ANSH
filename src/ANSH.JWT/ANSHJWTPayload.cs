using System;
using Newtonsoft.Json;

namespace ANSH.JWT {
    /// <summary>
    /// JWT有效载荷
    /// </summary>
    public class ANSHJWTPayload {

        /// <summary>
        /// 发行人
        /// </summary>
        [JsonProperty ("iss")]
        public string Iss { get; set; }

        /// <summary>
        /// 到期时间，时间戳
        /// </summary>
        [JsonProperty ("exp")]
        public double? Exp { get; set; } = DateTime.Now.AddMinutes (20).ToTimeStamp ();

        /// <summary>
        /// 在此时间之前不可用，时间戳
        /// </summary>
        [JsonProperty ("nbf")]
        public double? Nbf { get; set; } = DateTime.Now.ToTimeStamp ();

        /// <summary>
        ///发布时间，时间戳
        /// </summary>
        [JsonProperty ("iat")]
        public double? Iat { get; set; } = DateTime.Now.ToTimeStamp ();

        /// <summary>
        /// 主题
        /// </summary>
        [JsonProperty ("sub")]
        public string Sub { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [JsonProperty ("aud")]
        public string Aud { get; set; }

        /// <summary>
        /// JWT ID用于标识该JWT
        /// </summary>
        [JsonProperty ("jti")]
        public string Jti { get; set; }

    }
}