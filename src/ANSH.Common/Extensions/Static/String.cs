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
public static class ANSHCommonExtensionsString {
    /// <summary>
    /// 隐私保护字符串
    /// </summary>
    /// <param name="txt">需要保护的字符串</param>
    /// <param name="length">从第几个字符开始隐匿</param>
    /// <returns>隐匿后的字符串</returns>
    public static string PrivacyTxt (this string txt, int length) {
        if (txt?.Length > length) {
            StringBuilder item = new StringBuilder ();
            for (int i = 0; i < txt.Length - length; i++) {
                item.Append ("*");
            }
            return txt.SubStrChar (length, item.ToString ());
        } else if (txt?.Length >= 1) {
            return txt.SubStrChar (txt.Length - 1, "*");
        } else {
            return txt;
        }
    }

    /// <summary>
    /// 隐私保护字符串
    /// </summary>
    /// <param name="txt">需要保护的字符串</param>
    /// <param name="beginReserved">起始位置保留字符数</param>
    /// <param name="endReserved">结束位置保留字符数</param>
    /// <param name="least">至少需要隐匿多少个字符数</param>
    /// <returns>隐匿后的字符串</returns>
    public static string PrivacyTxtMiddle (this string txt, int beginReserved, int endReserved, int least) {
        StringBuilder item = new StringBuilder ();
        if (txt?.Length >= beginReserved + endReserved + least) {
            for (int i = 0; i < txt.Length - beginReserved - endReserved; i++) {
                item.Append ("*");
            }
            return $"{txt.Substring (0, beginReserved)}{item.ToString()}{txt.Substring (txt.Length - endReserved)}";
        } else {
            return PrivacyTxt (txt, 1);
        }
    }

    /// <summary>
    /// 隐私保护字符串（区域）
    /// </summary>
    /// <param name="address">区域，省 市 区 详细地址</param>
    /// <param name="beginReserved">起始位置保留字符数</param>
    /// <param name="endReserved">结束位置保留字符数</param>
    /// <param name="least">至少需要隐匿多少个字符数</param>
    /// <returns>隐匿后的字符串</returns>
    public static string PrivacyTxtAddress (this string address, int beginReserved, int endReserved, int least) {
        var addresses = address?.Split (' ');

        int length = addresses?.Length??0;

        int removeCount = length >= 4 ? 3 : length - 1;

        var _addresses = addresses.ToList ();

        if (removeCount > 0) {
            _addresses.RemoveRange (0, 3);
        }

        StringBuilder result = new StringBuilder ();

        for (int i = 0; i < removeCount; i++) {
            result.Append ($"{addresses[i]?.Trim()} ");
        }

        result.Append ($"{PrivacyTxtMiddle (string.Join (' ', _addresses),beginReserved,endReserved,least)?.Trim()}");

        return result.ToString ();
    }

    /// <summary>
    /// 隐私保护字符串（区域）
    /// </summary>
    /// <param name="address">区域，省 市 区 详细地址</param>
    /// <returns>隐匿后的字符串</returns>
    public static string PrivacyTxtAddress (this string address) => PrivacyTxtAddress (address, 3, 3, 5);

    /// <summary>
    /// 隐私保护字符串（名称）
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns>隐匿后的字符串</returns>
    public static string PrivacyTxtName (this string name) {
        if (name?.Length == 2) {
            return PrivacyTxtMiddle (name, 0, 1, 1);
        }
        if (name?.Length == 3) {

            return PrivacyTxtMiddle (name, 0, 2, 1);
        }
        if (name?.Length == 4) {
            return PrivacyTxtMiddle (name, 0, 2, 2);
        }
        if (name?.Length > 4) {
            return PrivacyTxtMiddle (name, 0, (int) name?.Length / 2, 2);
        }
        return PrivacyTxtMiddle (name, 0, 0, 1);
    }

    /// <summary>
    /// 隐私保护字符串（电话号码）
    /// </summary>
    /// <param name="phone">电话号码</param>
    /// <returns>隐匿后的字符串</returns>
    public static string PrivacyTxtPhone (this string phone) => PrivacyTxtMiddle (phone, 3, 4, 4);

    /// <summary>
    /// 获取拼音首字母
    /// </summary>
    /// <param name="value">汉字字符串</param>
    /// <returns>相对应的汉语拼音首字母串</returns>
    public static string ToPingYinInitial (this string value) {
        string strTemp = "";
        int iLen = value?.Length??0;
        int i = 0;
        for (i = 0; i <= iLen - 1; i++) {
            strTemp += GetPingYinInitial (value.Substring (i, 1)).ToUpper ();
        }
        return strTemp;
    }

    /// <summary>
    /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母
    /// </summary>
    /// <param name="cnChar">单个汉字</param>
    /// <returns>单个大写字母</returns>
    static string GetPingYinInitial (string cnChar) {
        System.Text.Encoding.RegisterProvider (System.Text.CodePagesEncodingProvider.Instance);
        byte[] arrCN = Encoding.GetEncoding ("gb2312").GetBytes (cnChar);
        if (arrCN.Length > 1) {
            int area = (short) arrCN[0];
            int pos = (short) arrCN[1];
            int code = (area << 8) + pos;
            int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
            for (int i = 0; i < 26; i++) {
                int max = 55290;
                if (i != 25) max = areacode[i + 1];
                if (areacode[i] <= code && code < max) {
                    return Encoding.Default.GetString (new byte[] {
                        (byte) (65 + i)
                    });
                }
            }
            return " ";
        } else {
            return cnChar;
        };
    }

    /// <summary>
    /// 解析身份证号
    /// </summary>
    /// <param name="value">身份证号码</param>
    /// <param name="birthday">生日</param>
    /// <param name="age">年龄</param>
    /// <param name="gender">性别</param>
    public static void ANSHAnalyzeIdentityNo (this string value, out DateTime birthday, out int age, out string gender) {
        DateTime now = DateTime.Now;
        if (!value.RegexCheck (ANSHRegexType.身份证)) {
            throw new Exception ("未能识别的证件号");
        }

        if (value.Length == 18) {
            gender = Convert.ToBoolean (value.Substring (16, 1).ToInt () % 2) ? "男" : "女";
            birthday = DateTime.Parse ($"{value.Substring (6, 4)}-{value.Substring (10, 2)}-{value.Substring (12, 2)}");
        } else {
            gender = Convert.ToBoolean (value.Substring (14, 1).ToInt () % 2) ? "男" : "女";
            birthday = DateTime.Parse ($"19{value.Substring (6, 2)}-{value.Substring (8, 2)}-{value.Substring (10, 2)}");
        }
        
        age = (now.Year - birthday.Year);
        if (now.Month < birthday.Month || (now.Month == birthday.Month && now.Day < birthday.Day)) {
            age--;
        }
    }
}