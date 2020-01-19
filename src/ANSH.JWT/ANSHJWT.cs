using System;
using System.Linq;
using System.Text;

namespace ANSH.JWT {
    /// <summary>
    /// JWT操作类
    /// </summary>
    public class ANSHJWT {

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="payload">有效载荷</param>
        /// <param name="secretKey">密匙</param>
        /// <param name="hashAlgorithm">加密方式</param>
        /// <returns>jwt字符串</returns>
        public static string Encode (ANSHJWTPayload payload, string secretKey, ANSHJWTHashAlgorithm hashAlgorithm = ANSHJWTHashAlgorithm.HS256) {
            var header = new ANSHJWTHeader ();
            string headerUrlEncode = header.ToJson ().ToBase64UrlString (Encoding.UTF8);
            string payloadEncode = payload.ToJson ().ToBase64UrlString (Encoding.UTF8);
            string signatureEncode = $"{headerUrlEncode}.{payloadEncode}".HMACSHA256Encryp (secretKey).ToBase64UrlString ();
            return $"{headerUrlEncode}.{payloadEncode}.{signatureEncode}";
        }

        /// <summary>
        /// 解码并验证
        /// </summary>
        /// <param name="jwtString">jwt字符串</param>
        /// <param name="secretKey">密匙</param>
        /// <param name="outPayload">有效载荷</param>
        /// <returns>成功返回true，失败返回false</returns>
        public static bool Decode<TANSHJWTPayload> (string jwtString, string secretKey, out TANSHJWTPayload outPayload)
        where TANSHJWTPayload : ANSHJWTPayload {
            outPayload = default (TANSHJWTPayload);
            if (!AnalyzeJWTInfo (jwtString, out ANSHJWTHeader outHeader, out outPayload, out string outSignature)) {
                return false;
            }

            if (jwtString != Encode (outPayload, secretKey)) {
                return false;
            }

            if (DateTime.Now.ToTimeStamp () >= outPayload.Exp) {
                return false;
            }

            if (DateTime.Now.ToTimeStamp () <= outPayload.Nbf) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 解析JWT字符串
        /// </summary>
        /// <param name="jwtString">jwt字符串</param>
        /// <param name="header">JWT头</param>
        /// <param name="payload">JWT有效载荷</param>
        /// <param name="signature">签名</param>
        /// <returns>成功返回true，失败返回false</returns>
        public static bool AnalyzeJWTInfo<TANSHJWTPayload> (string jwtString, out ANSHJWTHeader header, out TANSHJWTPayload payload, out string signature)
        where TANSHJWTPayload : ANSHJWTPayload {
            header = null;
            payload = null;
            signature = null;
            var jwtArray = jwtString.Split (".", StringSplitOptions.RemoveEmptyEntries);

            if (jwtArray?.Length != 3) {
                return false;
            }

            header = jwtArray[0].FromBase64UrlString (Encoding.UTF8).ToJsonObj<ANSHJWTHeader> ();
            payload = jwtArray[1].FromBase64UrlString (Encoding.UTF8).ToJsonObj<TANSHJWTPayload> ();
            signature = jwtArray[2].FromBase64UrlString (Encoding.UTF8);

            return true;
        }
    }
}