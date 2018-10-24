using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// 拓展类方法
/// </summary>
public static class ANSHCommonExtensionsHMACSHA1 {

    /// <summary>
    /// HMACSHA1加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="key">密钥参数</param>
    /// <returns>SHA1加密数据值</returns>
    public static byte[] HMACSHA1Encryp (this string value, string key) => ASCIIEncoding.UTF8.GetBytes (value).HMACSHA1Encryp (key);

    /// <summary>
    /// HMACSHA1加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="key">密钥参数</param>
    /// <returns>SHA1加密数据值</returns>
    public static byte[] HMACSHA1Encryp (this byte[] value, string key) => new HMACSHA1 (ASCIIEncoding.UTF8.GetBytes (key)).ComputeHash (value);

    /// <summary>
    /// HMACSHA1加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="key">密钥参数</param>
    /// <returns>SHA1加密数据值</returns>
    public static byte[] HMACSHA1Encryp (this Stream value, string key) => value.ToByte ().HMACSHA1Encryp (key);

}