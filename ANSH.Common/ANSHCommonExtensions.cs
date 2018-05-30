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
public static class ANSHCommonExtensions {

    #region System.Int32
    /// <summary>
    /// 将此实例的值转换为 System.Int32。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <returns>与当前实例值对应的 System.Int32</returns>
    public static int ToInt (this string value, int? default_value = null) {
        bool is_parse = value.IsInt (out int? result);

        if (!is_parse && default_value == null) {
            throw new FormatException ("转换类型失败");
        }

        return is_parse ? result.Value : default_value.Value;
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.Int32。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回null</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsInt (this string value, out int? result) {
        bool is_parse = int.TryParse (value, out int parse_result);
        result = is_parse ? (int?) parse_result : null;
        return is_parse;
    }
    #endregion

    #region System.Enum
    /// <summary>
    /// 将此实例的值转换为指定的 System.Enum
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <returns>与当前实例值对应的 System.Enum</returns>
    public static T ToEnum<T> (this int value, T? default_value = null) where T : struct {
        bool is_parse = value.IsEnum (out T? result);

        if (!is_parse && default_value == null) {
            throw new FormatException ("转换类型失败");
        }

        return is_parse ? result.Value : default_value.Value;
    }

    /// <summary>
    /// 判断此实例的值是否可转换为指定的 System.Enum
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回null</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsEnum<T> (this int value, out T? result) where T : struct {
        bool is_parse = Enum.TryParse (value.ToString (), out T parse_result) && Enum.IsDefined (typeof (T), parse_result);
        result = is_parse ? (T?) parse_result : null;
        return is_parse;
    }
    #endregion

    #region System.Int64

    /// <summary>
    /// 将此实例的值转换为 System.Int64。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>与当前实例值对应的 System.Int64</returns>
    public static long ToLong (this string value) {
        return value.ToLong (null);
    }

    /// <summary>
    /// 将此实例的值转换为 System.Int64。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <returns>与当前实例值对应的 System.Int64</returns>
    public static long ToLong (this string value, long? default_value = null) {
        bool is_parse = value.IsLong (out long? result);

        if (!is_parse && default_value == null) {
            throw new FormatException ("转换类型失败");
        }

        return is_parse ? result.Value : default_value.Value;
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.Int64。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回null</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsLong (this string value, out long? result) {
        bool is_parse = long.TryParse (value, out long parse_result);

        result = is_parse ? (long?) parse_result : null;
        return is_parse;
    }
    #endregion

    #region System.DateTime 
    /// <summary>
    /// 将此实例的值转换为 System.DateTime。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <param name="format">指定DateTime转换格式。</param>
    /// <returns>与当前实例值对应的 System.DateTime</returns>
    public static DateTime ToDateTime (this string value, DateTime? default_value = null, string format = null) {
        bool is_parse = value.IsDateTime (out DateTime? result, format);

        if (!is_parse && default_value == null) {
            throw new FormatException ("转换类型失败");
        }

        return is_parse ? result.Value : default_value.Value;
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.DateTime。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回null</param>
    /// <param name="format">指定DateTime转换格式。</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsDateTime (this string value, out DateTime? result, string format = null) {
        bool is_parse = format == null ? DateTime.TryParse (value, out DateTime parse_result) : DateTime.TryParseExact (value, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parse_result);

        result = is_parse ? (DateTime?) parse_result : null;
        return is_parse;
    }
    #endregion

    #region System.Decimal
    /// <summary>
    /// 将此实例的值转换为 System.Decimal。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <returns>与当前实例值对应的 System.Decimal</returns>
    public static decimal ToDecimal (this string value, decimal? default_value = null) {
        var is_parse = value.IsDecimal (out decimal? result, null);

        if (!is_parse && default_value == null) {
            throw new FormatException ("转换类型失败");
        }
        return is_parse ? result.Value : default_value.Value;
    }

    /// <summary>
    /// 此实例的值保留指定小数位数
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="digit">指定Decimal小数位数。</param>
    /// <returns>保留指定小数位数后的实例值。</returns>
    public static decimal ToFixed (this decimal value, int digit) {
        return decimal.Parse (value.ToString (DecimalDigit (digit)));
    }

    /// <summary>
    /// 判断此实例的值是否含指定小数位数
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="digit">指定Decimal小数位数。</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsFixed (this decimal value, int digit) {
        return value.ToString ().IsDecimal (out decimal? result, digit);
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.Decimal。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回null</param>
    /// <param name="digit">指定Decimal小数位数。</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsDecimal (this string value, out decimal? result, int? digit = null) {
        result = null;
        bool is_parse = decimal.TryParse (value, out decimal parse_result);
        string[] split_digit;
        if (!is_parse || (split_digit = value.Split ('.')).Length > 2) {
            return false;
        } else if (digit != null && split_digit.Length == 2) {
            while (split_digit[1].Length != 0 && split_digit[1].LastIndexOf ("0") == (split_digit[1].Length - 1)) {
                split_digit[1] = split_digit[1].TrimEnd ('0');
            }
            if (split_digit[1].Length > digit) {
                return false;
            }
        }

        result = parse_result;
        if (digit.HasValue) {
            result = (decimal?) decimal.Parse (parse_result.ToString (DecimalDigit (digit.Value)));
        }
        return true;
    }

    /// <summary>
    /// 获取Decimal格式
    /// </summary>
    /// <param name="digit">保留几位小数</param>
    /// <returns>Decimal格式</returns>
    static string DecimalDigit (int digit) {
        string str_digit = "0.";
        for (int i = 0; i < digit; i++) {
            str_digit += "0";
        }
        return str_digit;
    }
    #endregion

    #region FromBase64String
    /// <summary>
    /// 将此实例值（它将二进制数据编码为 Base64 数字）转换为等效的8位无符号整数数组
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns> 与此实例值等效的8位无符号整数数组</returns>
    public static byte[] FromBase64 (this String value) {
        return Convert.FromBase64String (value);
    }
    /// <summary>
    /// 将此实例值（它将二进制数据编码为 Base64 数字）转换为等效的8位无符号整数数组
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="encoding">编码</param>
    /// <returns> 与此实例值等效的8位无符号整数数组</returns>
    public static string FromBase64String (this String value, Encoding encoding) {
        return encoding.GetString (value.FromBase64 ());
    }
    /// <summary>
    /// 将此实例值为其用Base64数字编码的等效字符串表示形式
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns> 与此实例值字符串表示形式，以Base64表示</returns>
    public static string ToBase64String (this byte[] value) {
        return Convert.ToBase64String (value);
    }

    /// <summary>
    /// 将此实例值为其用Base64数字编码的等效字符串表示形式
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="encoding">编码</param>
    /// <returns> 与此实例值字符串表示形式，以Base64表示</returns>
    public static string ToBase64String (this string value, Encoding encoding) {
        return Convert.ToBase64String (encoding.GetBytes (value));
    }

    #endregion

    #region System.Byte[]
    /// <summary>
    /// 将此实例的值转换为 System.Byte[]。
    /// </summary>
    /// <param name="value">当前实例值</param>
    public static byte[] ToByte (this Stream value) {
        if (!value?.CanRead??false) {
            throw new NotSupportedException ("Stream不能读取");
        }
        if (value.Position != 0) {
            throw new Exception ("Stream位置为0");
        }
        byte[] bytes = new byte[value.Length];
        value.Read (bytes, 0, bytes.Length);
        return bytes;
    }
    #endregion

    #region Syste.IO.Stream
    /// <summary>
    /// 将此实例的值转换为 Syste.IO.Stream 。
    /// </summary>
    /// <param name="value">当前实例值</param>
    public static Stream ToStream (this byte[] value) {
        Stream stream = new MemoryStream (value);
        return stream;
    }
    #endregion

    #region HtmlEncode
    /// <summary>
    /// HTML加密
    /// </summary>
    /// <param name="value">要加密的字符串</param>
    /// <returns>加密后的字符串</returns>
    public static string HtmlEncode (this string value) {
        return WebUtility.HtmlEncode (value);
    }

    #endregion

    #region HtmlDecode
    /// <summary>
    /// HTML解密
    /// </summary>
    /// <param name="value">要解密的字符串</param>
    /// <returns>解密后的字符串</returns>
    public static string HtmlDecode (this string value) {
        return WebUtility.HtmlDecode (value);
    }
    #endregion

    #region UrlEncode
    /// <summary>
    /// Url加密
    /// </summary>
    /// <param name="value">要加密的字符串</param>
    /// <returns>加密后的字符串</returns>
    public static string UrlEncode (this string value) {
        return WebUtility.UrlEncode (value);
    }
    #endregion

    #region UrlDecode
    /// <summary>
    /// Url解密
    /// </summary>
    /// <param name="value">要解密的字符串</param>
    /// <returns>解密后的字符串</returns>
    public static string UrlDecode (this string value) {
        return WebUtility.UrlDecode (value);
    }
    #endregion

    #region Extension
    /// <summary>
    /// 获取扩展名
    /// <param name="value">当前实例值</param>
    /// </summary>
    public static string GetExtension (this string value) {
        return (value ?? string.Empty).Split ('.').Last ();
    }
    #endregion

    #region MD5Encryp
    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>MD5加密数据值</returns>
    public static string MD5Encryp (this string value) {
        return ASCIIEncoding.UTF8.GetBytes (value).MD5Encryp ();
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>MD5加密数据值</returns>
    public static string MD5Encryp (this byte[] value) {
        byte[] tmpHash = MD5.Create ().ComputeHash (value);
        StringBuilder md5 = new StringBuilder ();
        foreach (byte bt in tmpHash) {
            md5.Append (bt.ToString ("x2"));
        }
        return md5.ToString ();
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>MD5加密数据值</returns>
    public static string MD5Encryp (this Stream value) {
        return value.ToByte ().MD5Encryp ();
    }
    #endregion

    #region DesEncrypt

    /// <summary>
    /// 密匙
    /// </summary>
    private static string _DESKey = "3FF1E929BC0534950B0920A7B59FA698BD02DFE8";

    /// <summary> 
    /// DES加密数据 
    /// </summary> 
    /// <param name="value">当前实例值</param>
    /// <returns>DES加密数据值</returns> 
    public static string DESEncrypt (this string value) {
        var des = System.Security.Cryptography.TripleDES.Create ();
        byte[] inputByteArray = Encoding.UTF8.GetBytes (value);
        des.Key = ASCIIEncoding.ASCII.GetBytes (_DESKey);
        des.IV = ASCIIEncoding.ASCII.GetBytes (_DESKey);
        System.IO.MemoryStream ms = new System.IO.MemoryStream ();
        CryptoStream cs = new CryptoStream (ms, des.CreateEncryptor (), CryptoStreamMode.Write);
        cs.Write (inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock ();
        StringBuilder ret = new StringBuilder ();
        foreach (byte b in ms.ToArray ()) {
            ret.AppendFormat ("{0:X2}", b);
        }
        return ret.ToString ();
    }

    /// <summary> 
    /// DES解密数据 
    /// </summary> 
    /// <param name="value">当前实例值</param>
    /// <returns>DES解密数据值</returns> 
    public static string DESDecrypt (this string value) {
        var des = System.Security.Cryptography.TripleDES.Create ();
        int len;
        len = value.Length / 2;
        byte[] inputByteArray = new byte[len];
        int x, i;
        for (x = 0; x < len; x++) {
            i = Convert.ToInt32 (value.Substring (x * 2, 2), 16);
            inputByteArray[x] = (byte) i;
        }

        des.Key = ASCIIEncoding.ASCII.GetBytes (_DESKey);
        des.IV = ASCIIEncoding.ASCII.GetBytes (_DESKey);
        System.IO.MemoryStream ms = new System.IO.MemoryStream ();
        CryptoStream cs = new CryptoStream (ms, des.CreateDecryptor (), CryptoStreamMode.Write);
        cs.Write (inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock ();
        return Encoding.UTF8.GetString (ms.ToArray ());
    }
    #endregion

    #region 替换字符串
    /// <summary>
    /// 替换字符串
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="startIndex">起始位置</param>
    /// <param name="length">长度</param>
    /// <param name="newVaule">替换值</param>
    /// <returns>处理后的信息</returns>
    public static string SubReplace (this String value, int startIndex, int length, string newVaule) {
        string result = "";
        value = value.Trim ();
        result += value.Substring (0, startIndex + 1);
        result += newVaule;
        result += value.Substring (length + startIndex + 1);
        return result;
    }
    #endregion

    #region 截取字符串

    /// <summary>
    /// 从此实例检索子字符串。子字符串从指定的字符位置开始。
    /// <para>字节</para>
    /// </summary>
    /// <param name="length">要截取的字节长度</param>
    /// <param name="value">当前实例值</param>
    /// <returns>与value等效的指定引用类型值，如果value为null是返回null</returns>
    public static string SubStrByte (this String value, int length) {
        return value.SubStrByte (length, "");
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
        if (string.IsNullOrWhiteSpace (value.Trim ())) return "";

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
        return value.SubStrChar (length, "");
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
        if (string.IsNullOrWhiteSpace (value.Trim ())) return "";

        return value.Length > length ? value.Substring (0, length) + endstr : value;
    }
    #endregion

    #region 时间戳
    /// <summary>
    /// 将此实例值转换成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日08时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>返回时间戳值</returns>
    public static long ToTimeStamp (this DateTime value) {
        return (long) Math.Floor (value.Subtract (TimeZoneInfo.ConvertTime (new DateTime (1970, 1, 1, 8, 0, 0), TimeZoneInfo.Local)).TotalSeconds);
    }

    /// <summary>
    /// 将此时间戳实例值转换成时间
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>返回时间戳对应DateTime</returns>
    public static DateTime ToTimeStamp (this long value) {
        return TimeZoneInfo.ConvertTime (new DateTime (1970, 1, 1, 8, 0, 0).AddSeconds (value), TimeZoneInfo.Local);
    }

    #endregion

    #region List<T>
    /// <summary>
    /// 与指定的泛型比较
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="compared">指定泛型</param>
    /// <param name="FuncCompare">指定比较交集的方法</param>
    /// <param name="intersect">交集</param>
    /// <param name="left_except">当前实例值与交集的差集</param>
    /// <param name="right_except">指定泛型与交集的差集</param>
    /// <returns></returns>
    public static void Compare<LEFT, RIGHT> (this List<LEFT> value, List<RIGHT> compared, Func<LEFT, RIGHT, bool> FuncCompare, out List<KeyValuePair<LEFT, RIGHT>> intersect, out List<LEFT> left_except, out List<RIGHT> right_except) {

        List<LEFT> left = value ?? new List<LEFT> ();
        List<RIGHT> right = compared ?? new List<RIGHT> ();

        intersect = new List<KeyValuePair<LEFT, RIGHT>> ();
        left_except = new List<LEFT> ();
        right_except = new List<RIGHT> ();

        foreach (var in_left in left) {
            foreach (var in_right in right) {
                if (FuncCompare (in_left, in_right)) {
                    intersect.Add (new KeyValuePair<LEFT, RIGHT> (in_left, in_right));
                }
            }
        }
        left_except = left.Except (intersect.Select (m => m.Key).ToList ()).ToList ();
        right_except = right.Except (intersect.Select (m => m.Value).ToList ()).ToList ();
    }
    #endregion

    /// <summary>
    /// 数据流编码格式
    /// </summary>
    /// <param name="buffer">当前实例值</param>
    /// <returns>返回编码格式</returns>
    public static System.Text.Encoding GetEncode (this byte[] buffer) {
        if (buffer[0] == 0xEF && buffer[1] == 0xBB) {
            return System.Text.Encoding.UTF8;
        } else if (buffer[0] == 0xFE && buffer[1] == 0xFF) {
            return System.Text.Encoding.BigEndianUnicode;
        } else if (buffer[0] == 0xFF && buffer[1] == 0xFE) {
            return System.Text.Encoding.Unicode;
        } else {
            return System.Text.Encoding.GetEncoding ("gb2312");
        }
    }
}