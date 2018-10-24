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
public static class ANSHCommonExtensionsINT32 {
    /// <summary>
    /// 将此实例的值转换为 System.Int32。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <exception cref="System.FormatException">当此实例不是有效的System.Enum且<paramref name="default_value"/>为null时引发异常。</exception>
    /// <returns>与当前实例值对应的 System.Int32</returns>
    public static int ToInt (this string value, int? default_value = null) {
        return value.IsInt (out int result) ?
            result :
            (
                default_value.HasValue ?
                default_value.Value :
                throw new FormatException ("转换类型失败")
            );
    }

    /// <summary>
    /// 将此实例的值转换为 System.Int32。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <returns>与当前实例值对应的 System.Int32</returns>
    public static int ToInt (this string value, int default_value) {
        return value.IsInt (out int result) ? result : default_value;
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.Int32。 
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsInt (this string value) {
        return value.IsInt (out _);
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.Int32。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回默认值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsInt (this string value, out int result) {
        bool is_parse = int.TryParse (value, out int parse_result);
        result = is_parse ? parse_result : default (int);
        return is_parse;
    }
}