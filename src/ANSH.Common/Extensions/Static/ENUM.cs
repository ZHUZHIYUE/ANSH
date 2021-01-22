using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// 拓展类方法
/// </summary>
public static class ANSHCommonExtensionsENUM {

    /// <summary>
    /// 将此实例的值转换为指定的 System.Enum
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <exception cref="System.FormatException">当此实例不是有效的System.Enum时引发异常。</exception>
    /// <returns>与当前实例值对应的 System.Enum</returns>
    public static T ToEnum<T> (this string value) where T : struct {
        return value.IsEnum (out T result) ?
            result : throw new FormatException ("转换类型失败");
    }

    /// <summary>
    /// 将此实例的值转换为 System.Int32。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <returns>与当前实例值对应的 System.Int32</returns>
    public static T ToEnum<T> (this string value, T default_value) where T : struct {
        return value.IsEnum (out T result) ? result : default_value;
    }

    /// <summary>
    /// 将此实例的值转换为指定的 System.Enum
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <exception cref="System.FormatException">当此实例不是有效的System.Enum时引发异常。</exception>
    /// <returns>与当前实例值对应的 System.Enum</returns>
    public static T ToEnum<T> (this int value) where T : struct {
        return value.IsEnum (out T result) ?
            result : throw new FormatException ("转换类型失败");
    }

    /// <summary>
    /// 将此实例的值转换为 System.Int32。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <returns>与当前实例值对应的 System.Int32</returns>
    public static int ToEnum<T> (this int value, int default_value) {
        return value.IsEnum (out int result) ? result : default_value;
    }

    /// <summary>
    /// 判断此实例的值是否可转换为指定的 System.Enum
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsEnum<T> (this int value) where T : struct {
        return value.IsEnum (out T _);
    }
    /// <summary>
    /// 判断此实例的值是否可转换为指定的 System.Enum
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsEnum<T> (this string value) where T : struct {
        return value.IsEnum (out T _);
    }

    /// <summary>
    /// 判断此实例的值是否可转换为指定的 System.Enum
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回默认值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsEnum<T> (this int value, out T result) where T : struct {
        return value.ToString ().IsEnum (out result);
    }
    /// <summary>
    /// 判断此实例的值是否可转换为指定的 System.Enum
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回默认值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsEnum<T> (this string value, out T result) where T : struct {
        bool is_parse = Enum.TryParse (value, out T parse_result) && Enum.IsDefined (typeof (T), parse_result);
        result = is_parse ? parse_result : default (T);
        return is_parse;
    }

    /// <summary>
    /// 获取枚举DisplayName值
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>DisplayName值</returns>
    public static string GetDisplayName<T> (this T value) where T : Enum {
        if (value == null) {
            return null;
        } else {
            var t = value.GetType ().GetFields ().SingleOrDefault (w => w.Name == value.ToString ()).CustomAttributes.SingleOrDefault (w => w.AttributeType == typeof (DisplayAttribute));
            if (t != null) {
                return t.NamedArguments[0].TypedValue.Value.ToString ();
            } else {
                return value.ToString ();
            }

        }
    }
}