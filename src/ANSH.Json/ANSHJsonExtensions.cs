using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

/// <summary>
/// 拓展类方法
/// </summary>
public static class ANSHJsonExtensions {
    /// <summary>
    /// 生成Json格式
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>返回Json格式字符串</returns>
    public static string ToJson (this Object value) {
        return JsonConvert.SerializeObject (value, new JsonSerializerSettings () { NullValueHandling = NullValueHandling.Ignore });
    }

    /// <summary>
    /// 生成Json格式
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>返回Json格式字符串</returns>
    public static string ToJsonCamelCase (this Object value) {
        return JsonConvert.SerializeObject (value, new JsonSerializerSettings () { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver () });
    }

    /// <summary>
    /// 将JSON 字符转换为对象
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>返回Json转化T类型</returns>
    public static T ToJsonObj<T> (this string value) {
        if (value != null) {
            return JsonConvert.DeserializeObject<T> (value.ToString ());
        }
        return default (T);
    }

    /// <summary>
    /// 将JSON 字符转换为对象
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="type">所需要转换的类型</param>
    /// <returns>返回Json转化T类型</returns>
    public static object ToJsonObj (this string value, Type type) {
        if (value != null) {
            return JsonConvert.DeserializeObject (value, type);
        }
        return null;
    }
}