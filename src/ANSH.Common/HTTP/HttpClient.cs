using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ANSH.Common.HTTP {
    /// <summary>
    /// HTTP通信实现库
    /// </summary>
    public class HTTPClient {
        #region GET
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        /// <param name="headers">默认标头</param>
        /// <param name="x509cert">安全证书</param>
        /// <returns>返回请求页面响应内容</returns>
        public static HttpResponseMessage Get (Uri url, Dictionary<string, string> headers = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None, params X509Certificate[] x509cert) {
            return GetAsync (url, headers, autDecom, x509cert).Result;
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        /// <param name="headers">默认标头</param>
        /// <param name="x509cert">安全证书</param>
        /// <returns>返回请求页面响应内容</returns>
        public static async Task<HttpResponseMessage> GetAsync (Uri url, Dictionary<string, string> headers = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None, params X509Certificate[] x509cert) {

            using (HttpClientHandler handler = new HttpClientHandler ()) {
                CreateHandlerDecompressionMethods (handler, autDecom);
                CreateHandlerX509 (handler, x509cert);

                using (HttpClient http = handler == null ? new HttpClient () : new HttpClient (handler)) {
                    AddHeaders (http, headers);
                    return await http.GetAsync (url);
                }
            }
        }
        #endregion

        #region POST

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">POST内容</param>
        /// <param name="name">HTTP内容的名称</param>
        /// <param name="filename">HTTP内容文件名</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        /// <param name="headers">默认标头</param>
        /// <param name="x509cert">安全证书</param>
        /// <returns>返回请求页面响应内容</returns>
        public static HttpResponseMessage PostFile (Uri url, byte[] param, string name, string filename, Dictionary<string, string> headers = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None, params X509Certificate[] x509cert) {
            return PostFileAsync (url, param, name, filename, headers, autDecom, x509cert).Result;
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">POST内容</param>
        /// <param name="name">HTTP内容的名称</param>
        /// <param name="filename">HTTP内容文件名</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        /// <param name="headers">默认标头</param>
        /// <param name="x509cert">安全证书</param>
        /// <returns>返回请求页面响应内容</returns>
        public static async Task<HttpResponseMessage> PostFileAsync (Uri url, byte[] param, string name, string filename, Dictionary<string, string> headers = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None, params X509Certificate[] x509cert) {
            using (MemoryStream stream = new MemoryStream (param)) {
                return await PostFileAsync (url, stream, name, filename, headers, autDecom, x509cert);
            }
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">POST内容</param>
        /// <param name="name">HTTP内容的名称</param>
        /// <param name="filename">HTTP内容文件名</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        /// <param name="headers">默认标头</param>
        /// <param name="x509cert">安全证书</param>
        /// <returns>返回请求页面响应内容</returns>
        public static HttpResponseMessage PostFile (Uri url, Stream param, string name, string filename, Dictionary<string, string> headers = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None, params X509Certificate[] x509cert) {
            return PostFileAsync (url, param, name, filename, headers, autDecom, x509cert).Result;
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">POST内容</param>
        /// <param name="name">HTTP内容的名称</param>
        /// <param name="filename">HTTP内容文件名</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        /// <param name="headers">默认标头</param>
        /// <param name="x509cert">安全证书</param>
        /// <returns>返回请求页面响应内容</returns>
        public static async Task<HttpResponseMessage> PostFileAsync (Uri url, Stream param, string name, string filename, Dictionary<string, string> headers = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None, params X509Certificate[] x509cert) {
            using (MultipartFormDataContent content = new MultipartFormDataContent ()) {
                using (StreamContent file = new StreamContent (param)) {
                    content.Add (file, name, filename);
                    return await PostMultipartFormDataAsync (url, content, headers, autDecom, x509cert);
                }
            }
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="content">POST内容</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        /// <param name="headers">默认标头</param>
        /// <param name="x509cert">安全证书</param>
        /// <returns>返回请求页面响应内容</returns>
        public static async Task<HttpResponseMessage> PostMultipartFormDataAsync (Uri url, MultipartFormDataContent content, Dictionary<string, string> headers = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None, params X509Certificate[] x509cert) {
            if (content is null) {
                throw new ArgumentNullException ("$param值不能为空。");
            }
            using (HttpClientHandler handler = new HttpClientHandler ()) {
                CreateHandlerDecompressionMethods (handler, autDecom);
                CreateHandlerX509 (handler, x509cert);
                using (HttpClient http = handler == null ? new HttpClient () : new HttpClient (handler)) {
                    return await http.PostAsync (url, content);
                }
            }
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">POST内容</param>
        /// <param name="headers">默认标头</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        /// <param name="x509cert">安全证书</param>
        /// <returns>返回请求页面响应内容</returns>
        public static HttpResponseMessage PostFormUrlEncoded (Uri url, KeyValuePair<String, String>[] param, Dictionary<string, string> headers = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None, params X509Certificate[] x509cert) {
            return PostFormUrlEncodedAsync (url, param, headers, autDecom, x509cert).Result;
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">POST内容</param>
        /// <param name="headers">默认标头</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        /// <param name="x509cert">安全证书</param>
        /// <returns>返回请求页面响应内容</returns>
        public static async Task<HttpResponseMessage> PostFormUrlEncodedAsync (Uri url, KeyValuePair<String, String>[] param, Dictionary<string, string> headers = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None, params X509Certificate[] x509cert) {
            using (HttpClientHandler handler = new HttpClientHandler ()) {
                CreateHandlerDecompressionMethods (handler, autDecom);
                CreateHandlerX509 (handler, x509cert);
                using (HttpClient http = handler == null ? new HttpClient () : new HttpClient (handler)) {
                    AddHeaders (http, headers);
                    return await http.PostAsync (url, new FormUrlEncodedContent (param));
                }
            }
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">POST内容</param>
        /// <param name="mediaType">要用于该内容的媒体（默认：application/json;charset=utf-8）</param>
        /// <param name="headers">默认标头</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        /// <param name="x509cert">安全证书</param>
        /// <returns>返回请求页面响应内容</returns>
        public static HttpResponseMessage Post (Uri url, string param, MediaTypeHeaderValue mediaType, Dictionary<string, string> headers = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None, params X509Certificate[] x509cert) {
            return PostAsync (url, param, mediaType, headers, autDecom, x509cert).Result;
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">POST内容</param>
        /// <param name="mediaType">要用于该内容的媒体（默认：application/json;charset=utf-8）</param>
        /// <param name="headers">默认标头</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        /// <param name="x509cert">安全证书</param>
        /// <returns>返回请求页面响应内容</returns>
        public static async Task<HttpResponseMessage> PostAsync (Uri url, string param, MediaTypeHeaderValue mediaType, Dictionary<string, string> headers = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None, params X509Certificate[] x509cert) {
            using (HttpClientHandler handler = new HttpClientHandler ()) {
                CreateHandlerDecompressionMethods (handler, autDecom);
                CreateHandlerX509 (handler, x509cert);
                using (HttpClient http = handler == null ? new HttpClient () : new HttpClient (handler)) {
                    AddHeaders (http, headers);
                    return await http.PostAsync (url, new StringContent (param, Encoding.GetEncoding (mediaType.CharSet), mediaType.MediaType));
                }
            }
        }

        /// <summary>
        /// 向指定的HttpClientHandler加入安全证书
        /// </summary>
        /// <param name="handler">指定HttpClientHandler</param>
        /// <param name="x509cert">安全证书</param>
        static void CreateHandlerX509 (HttpClientHandler handler = null, params X509Certificate[] x509cert) {
            handler = handler ?? new HttpClientHandler ();
            if (x509cert != null && x509cert.Length > 0) {
                handler = new HttpClientHandler {
                ClientCertificateOptions = ClientCertificateOption.Manual
                };
                handler.ClientCertificates.AddRange (x509cert);
            }
        }

        /// <summary>
        /// 向指定的HttpClientHandler加入文件压缩解压编码格式
        /// </summary>
        /// <param name="handler">指定HttpClientHandler</param>
        /// <param name="autDecom">文件压缩解压编码格式</param>
        static void CreateHandlerDecompressionMethods (HttpClientHandler handler = null, System.Net.DecompressionMethods autDecom = System.Net.DecompressionMethods.None) {
            handler = handler ?? new HttpClientHandler ();
            handler.AutomaticDecompression = autDecom;
        }

        /// <summary>
        /// 向指定的HttpClient加入新的标头
        /// </summary>
        /// <param name="client">指定HttpClient</param>
        /// <param name="headers">标头集合，key：标头名称，value：标头值</param>
        static void AddHeaders (HttpClient client, Dictionary<string, string> headers) {
            if (client != null && client.DefaultRequestHeaders != null) {
                foreach (var item in headers ?? new Dictionary<string, string> ()) {
                    client.DefaultRequestHeaders.Add (item.Key, item.Value);
                }
            }
        }
        #endregion
    }
}