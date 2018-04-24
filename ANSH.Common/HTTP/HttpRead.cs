using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ANSH.Common.HTTP
{
    /// <summary>
    /// HTTP读取库
    /// </summary>
    public class HTTPRead
    {
        /// <summary>
        /// 读取HttpResponseMessage信息
        /// </summary>
        /// <param name="request">HttpResponseMessage信息</param>
        /// <returns>返回HttpResponseMessage信息</returns>
        public static string ReadBodyString(HttpResponseMessage request)
        {
            return request.Content.ReadAsStringAsync().Result;
        }


        /// <summary>
        /// 读取HttpResponseMessage信息
        /// </summary>
        /// <param name="request">HttpResponseMessage信息</param>
        /// <returns>返回HttpResponseMessage信息</returns>
        public static byte[] ReadBodyByte(HttpResponseMessage request)
        {
            return request.Content.ReadAsByteArrayAsync().Result;
        }


        /// <summary>
        /// 读取HttpResponseMessage信息
        /// </summary>
        /// <param name="request">HttpResponseMessage信息</param>
        /// <returns>返回HttpResponseMessage信息</returns>
        public static Stream ReadBodyStream(HttpResponseMessage request)
        {
            return request.Content.ReadAsStreamAsync().Result;
        }
    }
}
