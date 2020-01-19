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
public static class ANSHCommonExtensionsBase64Url {

    /// <summary>
    /// 将此实例值为其用Base64Url数字编码的等效字符串表示形式
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="encoding">编码</param>
    /// <returns> 与此实例值字符串表示形式，以Base64Url表示</returns>
    public static string ToBase64UrlString (this string value, Encoding encoding) {
        return encoding.GetBytes (value).ToBase64UrlString ();
    }

    /// <summary>
    /// 将此实例值为其用Base64Url数字编码的等效字符串表示形式
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns> 与此实例值字符串表示形式，以Base64Url表示</returns>
    public static string ToBase64UrlString (this byte[] value) {
        return Convert.ToBase64String (value).Replace ("+", "-").Replace ("/", "_").Replace ("=", "");
    }

    /// <summary>
    /// 将此实例值（它将二进制数据编码为 Base64Url 数字）转换为等效的8位无符号整数数组
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns> 与此实例值等效的8位无符号整数数组</returns>
    public static byte[] FromBase64Url (this string value) {
        return Convert.FromBase64String (value);
    }

    /// <summary>
    /// 将此实例值（它将二进制数据编码为 Base64Url 数字）转换为等效的8位无符号整数数组
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="encoding">编码</param>
    /// <returns> 与此实例值等效的8位无符号整数数组</returns>
    public static string FromBase64UrlString (this string value, Encoding encoding) {
        int base64factor = 4;
        var remainder = value.Length % base64factor;
        string base64String = value;
        if (remainder > 0) {
            for (int i = 0; i < 4 - remainder; i++) {
                base64String += "=";
            }
        }
        base64String = base64String.Replace ("-", "+").Replace ("_", "/");
        return encoding.GetString (base64String.FromBase64Url ());
    }
}