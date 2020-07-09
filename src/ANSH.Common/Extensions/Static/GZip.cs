using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// 拓展类方法
/// </summary>
public static class ANSHCommonExtensionsGZip {

    /// <summary>
    /// GZip编码
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="encoding">编码</param>
    /// <returns>编码后的8位无符号整数数组</returns>
    public static byte[] GZipCompress (this String value, Encoding encoding = null) {
        byte[] bytes = value.ToByte (encoding??Encoding.UTF8);
        using (var ms = new MemoryStream ()) {
            using (var zs = new GZipStream (ms, CompressionMode.Compress, true)) {
                zs.Write (bytes, 0, bytes.Length);
            }
            return ms.ToArray ();
        }
    }

    /// <summary>
    /// GZip解码
    /// </summary>
    /// <param name="value">当前实例</param>
    /// <param name="encoding">编码</param>
    /// <returns>解码后的字符串</returns>
    public static string GZipDeCompression (this byte[] value, Encoding encoding = null) {
        using (MemoryStream ms = new MemoryStream (value)) {
            using (GZipStream zs = new GZipStream (ms, CompressionMode.Decompress)) {
                return zs.ToByte ().FromByteToString (encoding??Encoding.UTF8);
            }
        }
    }
}