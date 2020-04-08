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
        public static string Encode<Tclaims> (ANSHJWTPayload<Tclaims> payload, string secretKey, ANSHJWTHashAlgorithm hashAlgorithm = ANSHJWTHashAlgorithm.HS256) {
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
        public static bool Decode<Tclaims> (string jwtString, string secretKey, out ANSHJWTPayload<Tclaims> outPayload) {
            outPayload = default (ANSHJWTPayload<Tclaims>);
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
        public static bool AnalyzeJWTInfo<Tclaims> (string jwtString, out ANSHJWTHeader header, out ANSHJWTPayload<Tclaims> payload, out string signature) {
            header = null;
            payload = null;
            signature = null;
            try {
                var jwtArray = jwtString.Split (".", StringSplitOptions.RemoveEmptyEntries);

                if (jwtArray?.Length != 3) {
                    return false;
                }

                header = jwtArray[0].FromBase64UrlString (Encoding.UTF8).ToJsonObj<ANSHJWTHeader> ();
                payload = jwtArray[1].FromBase64UrlString (Encoding.UTF8).ToJsonObj<ANSHJWTPayload<Tclaims>> ();
                signature = jwtArray[2].FromBase64UrlString (Encoding.UTF8);
                return true;
            } catch {
                return false;
            }
        }
    }
}