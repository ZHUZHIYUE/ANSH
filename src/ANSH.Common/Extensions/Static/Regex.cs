using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// 拓展类方法
/// </summary>
public static class ANSHCommonExtensionsRegex {

    /// <summary>
    /// 正则表达式验证
    /// </summary>
    /// <param name="value">需验证的字符串</param>
    /// <param name="regexString">正则表达式字符串</param>
    /// <returns>是否通过验证</returns>
    public static bool RegexCheck (this string value, string regexString) => new Regex (regexString).IsMatch (value);

    /// <summary>
    /// 正则表达式验证
    /// </summary>
    /// <param name="value">需验证的字符串</param>
    /// <param name="regexType">正则表达式规则枚举</param>
    /// <returns>是否通过验证</returns>
    public static bool RegexCheck (this string value, ANSHRegexType regexType) => value.RegexCheck (_RegexList[regexType]);

    /// <summary>
    /// /// 正则表达式集合
    /// </summary>
    static Dictionary<ANSHRegexType, string> _RegexList = new Dictionary<ANSHRegexType, string> () {
        [ANSHRegexType.空或正整数] = @"^[1-9]\d*$|(^\s*$)", [ANSHRegexType.文件名字符串] = "^[A-z0-9\u4e00-\u9fa5 ]{0,}$", [ANSHRegexType.正整数] = @"^[1-9]\d*$", [ANSHRegexType.身份证] = "(^[0-9]{15}$)|(^[0-9]{18}$)|(^[0-9]{17}([0-9]|X|x)$)", [ANSHRegexType.文件路径] = @"[a-zA-Z]:(\\([0-9a-zA-Z]+))+|(\/([0-9a-zA-Z]+))+", [ANSHRegexType.URL路径] = @"http(s)?://(([0-9]{1,3}\.){3}[0-9]{1,3}(:([0-9]{1,4}))?|([\w-]+\.)+[\w-]+)(/[\w- ./?%&=]*)?", [ANSHRegexType.密码] = "^(?![A-Za-z]+$)(?![A-Z\\d]+$)(?![A-Z\\W]+$)(?![a-z\\d]+$)(?![a-z\\W]+$)(?![\\d\\W]+$)\\S{6,16}$", [ANSHRegexType.手机号] = "^[1][356789][0-9]{9}$"
    };
}

/// <summary>
/// 正则表达式类型
/// </summary>
public enum ANSHRegexType {

    /// <summary>
    /// 空或正整数
    /// </summary>
    空或正整数,
    /// <summary>
    /// 正整数
    /// </summary>
    正整数,
    /// <summary>
    /// 非空且由中文、大小写字母、数字、中划线-、下划线_ 组成
    /// </summary>
    文件名字符串,
    /// <summary>
    /// 15-18位
    /// </summary>

    身份证,
    /// <summary>
    /// 文件路径
    /// </summary>

    文件路径,
    /// <summary>
    /// URL路径
    /// </summary>

    URL路径,
    /// <summary>
    /// 手机号
    /// </summary>

    手机号,
    /// <summary>
    /// 由数字、大写字母、小写字母、特殊符、至少其中三种组成，长度在6~16之间
    /// </summary>
    密码,
}