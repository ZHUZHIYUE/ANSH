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
    /// <exception cref="System.FormatException">当此实例不是有效的System.Int32时引发异常。</exception>
    /// <returns>与当前实例值对应的 System.Int32</returns>
    public static int ToInt (this string value) {
        return value.IsInt (out int result) ?
            result : throw new FormatException ("转换类型失败");
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

    /// <summary>
    /// 将此此实例值转换成分页相关数据
    /// </summary>
    /// <param name="dataCount">当前实例值，分页数据总条数</param>
    /// <param name="pageCount">满足指定条件数据可分页总数</param>
    /// <param name="hasNext">是否还有下一页</param>
    /// <param name="pageIndex">页数</param>
    /// <param name="pageSize">每页数据条数</param>
    public static void ToPage (this int dataCount, int pageIndex, int pageSize, out int pageCount, out bool hasNext) {
        pageCount = (int) Math.Ceiling (dataCount / (double) pageSize);
        hasNext = pageIndex < pageCount;
    }

}