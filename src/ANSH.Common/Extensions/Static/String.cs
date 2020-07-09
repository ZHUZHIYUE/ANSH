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

    #region 截取字符串

    /// <summary>
    /// 从此实例检索子字符串。子字符串从指定的字符位置开始。
    /// <para>字节</para>
    /// </summary>
    /// <param name="length">要截取的字节长度</param>
    /// <param name="value">当前实例值</param>
    /// <returns>与value等效的指定引用类型值，如果value为null是返回null</returns>
    public static string SubStrByte (this String value, int length) {
        return value?.SubStrByte (length, "");
    }

    /// <summary>
    /// 从此实例检索子字符串。子字符串从指定的字符位置开始。
    /// <para>字节</para>
    /// </summary>
    /// <param name="length">要截取的字节长度</param>
    /// <param name="endstr">后缀字符</param>
    /// <param name="value">当前实例值</param>
    /// <returns>与value等效的指定引用类型值，如果value为null是返回null</returns>
    public static string SubStrByte (this String value, int length, string endstr) {
        if (string.IsNullOrWhiteSpace (value)) return value;

        byte[] bytes = System.Text.Encoding.Unicode.GetBytes (value);
        int n = 0; //  表示当前的字节数
        int i = 0; //  要截取的字节数
        for (; i < bytes.GetLength (0) && n < length; i++) {
            //偶数位置，如0、2、4等，为UCS2编码中两个字节的第一个字节
            if (i % 2 == 0) {
                n++; //  在UCS2第一个字节时n加1
            } else {
                //  当UCS2编码的第二个字节大于0时，该UCS2字符为汉字，一个汉字算两个字节
                if (bytes[i] > 0) n++;
            }
        }
        //  如果i为奇数时，处理成偶数
        if (i % 2 == 1) {
            //  该UCS2字符是汉字时，去掉这个截一半的汉字
            if (bytes[i] > 0) i = i - 1; //  该UCS2字符是字母或数字，则保留该字符
            else i = i + 1;
        }
        return System.Text.Encoding.Unicode.GetString (bytes, 0, i) + (i < bytes.Length ? endstr : "");
    }

    /// <summary>
    /// 从此实例检索子字符串。子字符串从指定的字符位置开始。
    /// <para>字符</para>
    /// </summary>
    /// <param name="length">要截取的字符长度</param>
    /// <param name="value">当前实例值</param>
    /// <returns>与value等效的指定引用类型值，如果value为null是返回null</returns>
    public static string SubStrChar (this String value, int length) {
        return value?.SubStrChar (length, "");
    }

    /// <summary>
    /// 从此实例检索子字符串。子字符串从指定的字符位置开始。
    /// <para>字符</para>
    /// </summary>
    /// <param name="length">要截取的字符长度</param>
    /// <param name="endstr">后缀字符</param>
    /// <param name="value">当前实例值</param>
    /// <returns>与value等效的指定引用类型值，如果value为null是返回null</returns>
    public static string SubStrChar (this String value, int length, string endstr) {
        if (string.IsNullOrWhiteSpace (value)) return value;
        return value.Length > length ? value.Substring (0, length) + endstr : value;
    }
    #endregion

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
    public static bool ANSHAnalyzeIdentityNo (this string value, out DateTime birthday, out int age, out string gender) {
        age = 0;
        gender = "男";
        birthday = DateTime.Now;
        DateTime now = DateTime.Now;

        if (!value.RegexCheck (ANSHRegexType.身份证)) {
            return false;
        }

        if (value.Length == 18) {
            gender = Convert.ToBoolean (value.Substring (16, 1).ToInt () % 2) ? "男" : "女";
            if (!DateTime.TryParse ($"{value.Substring (6, 4)}-{value.Substring (10, 2)}-{value.Substring (12, 2)}", out birthday)) {
                return false;
            }
        } else {
            gender = Convert.ToBoolean (value.Substring (14, 1).ToInt () % 2) ? "男" : "女";
            if (!DateTime.TryParse ($"19{value.Substring (6, 2)}-{value.Substring (8, 2)}-{value.Substring (10, 2)}", out birthday)) {
                return false;
            }
        }

        age = (now.Year - birthday.Year);
        if (now.Month < birthday.Month || (now.Month == birthday.Month && now.Day < birthday.Day)) {
            age--;
        }

        if (age < 0) {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 将此8位无符号整数数组转换成字符串
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="encoding">编码</param>
    /// <returns> 与8位无符号整数数组等效的字符串</returns>
    public static string FromByteToString (this byte[] value, Encoding encoding = null) {
        return (encoding??Encoding.UTF8).GetString (value);
    }

    /// <summary>
    /// 将字符串转换成此8位无符号整数数组
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="encoding">编码</param>
    /// <returns> 8位无符号整数数组</returns>
    public static byte[] ToByte (this String value, Encoding encoding = null) {
        return (encoding??Encoding.UTF8).GetBytes (value);
    }
}