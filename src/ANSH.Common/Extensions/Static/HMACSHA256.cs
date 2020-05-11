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
public static class ANSHCommonExtensionsHMACSHA256 {

    /// <summary>
    /// HMACSHA256加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="key">密钥参数</param>
    /// <returns>SHA1加密数据值</returns>
    public static byte[] HMACSHA256Encryp (this string value, string key) => ASCIIEncoding.UTF8.GetBytes (value).HMACSHA256Encryp (key);

    /// <summary>
    /// HMACSHA256加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="key">密钥参数</param>
    /// <returns>SHA1加密数据值</returns>
    public static byte[] HMACSHA256Encryp (this byte[] value, string key) => new HMACSHA256 (ASCIIEncoding.UTF8.GetBytes (key)).ComputeHash (value);

    /// <summary>
    /// HMACSHA256加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>SHA1加密数据值</returns>
    public static byte[] SHA256 (this string value) => ASCIIEncoding.UTF8.GetBytes (value).SHA256 ();

    /// <summary>
    /// HMACSHA256加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>SHA1加密数据值</returns>
    public static byte[] SHA256 (this byte[] value) => System.Security.Cryptography.SHA256.Create ().ComputeHash (value);

    /// <summary>
    /// HMACSHA256加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="key">密钥参数</param>
    /// <returns>SHA1加密数据值</returns>
    public static byte[] HMACSHA256Encryp (this Stream value, string key) => value.ToByte ().HMACSHA256Encryp (key);

}