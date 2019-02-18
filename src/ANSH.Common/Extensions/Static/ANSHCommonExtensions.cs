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
    #region System.Double
    /// <summary>
    /// 将此实例的值转换为 System.Double。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <exception cref="System.FormatException">当此实例不是有效的System.Enum且<paramref name="default_value"/>为null时引发异常。</exception>
    /// <returns>与当前实例值对应的 System.Double</returns>
    public static double ToDouble (this string value, double? default_value = null) {
        return value.IsDouble (out double result) ?
            result :
            (
                default_value.HasValue ?
                default_value.Value :
                throw new FormatException ("转换类型失败")
            );
    }

    /// <summary>
    /// 将此实例的值转换为 System.Double。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <returns>与当前实例值对应的 System.Double</returns>
    public static double ToDouble (this string value, double default_value) {
        return value.IsDouble (out double result) ? result : default_value;
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.Double。 
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsDouble (this string value) {
        return value.IsDouble (out _);
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.Double。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回默认值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsDouble (this string value, out double result) {
        bool is_parse = double.TryParse (value, out double parse_result);
        result = is_parse ? parse_result : default (double);
        return is_parse;
    }
    #endregion

    #region System.Int64

    /// <summary>
    /// 将此实例的值转换为 System.Int64。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <exception cref="System.FormatException">当此实例不是有效的System.Enum且<paramref name="default_value"/>为null时引发异常。</exception>
    /// <returns>与当前实例值对应的 System.Int64</returns>
    public static long ToLong (this string value, long? default_value = null) {
        return value.IsLong (out long result) ?
            result :
            (
                default_value.HasValue ?
                default_value.Value :
                throw new FormatException ("转换类型失败")
            );
    }

    /// <summary>
    /// 将此实例的值转换为 System.Int64。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <returns>与当前实例值对应的 System.Int64</returns>
    public static long ToLong (this string value, long default_value) {
        return value.IsLong (out long result) ? result : default_value;
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.Int64。 
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsLong (this string value) {
        return value.IsLong (out _);
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.Int64。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回默认值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsLong (this string value, out long result) {
        bool is_parse = long.TryParse (value, out long parse_result);
        result = is_parse ? parse_result : default (long);
        return is_parse;
    }

    #endregion

    #region System.DateTime 
    /// <summary>
    /// 将此实例的值转换为 System.DateTime。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <exception cref="System.FormatException">当此实例不是有效的System.DateTime且<paramref name="default_value"/>为null时引发异常。</exception>
    /// <returns>与当前实例值对应的 System.DateTime</returns>
    public static DateTime ToDateTime (this string value, DateTime? default_value = null) {
        return value.IsDateTime (out DateTime result) ?
            result :
            (
                default_value.HasValue ?
                default_value.Value :
                throw new FormatException ("转换类型失败")
            );
    }

    /// <summary>
    /// 将此实例的值转换为 System.DateTime。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <param name="format">指定DateTime转换格式。</param>
    /// <exception cref="System.FormatException">当此实例不是格式为<paramref name="format"/>的有效System.DateTime且<paramref name="default_value"/>为null时引发异常。</exception>
    /// <returns>与当前实例值对应的 System.DateTime</returns>
    public static DateTime ToDateTime (this string value, DateTime? default_value, string format) {
        return value.IsDateTime (out DateTime result, format) ?
            result :
            (
                default_value.HasValue ?
                default_value.Value :
                throw new FormatException ("转换类型失败")
            );
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.DateTime。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsDateTime (this string value) {
        return value.IsDateTime (out _);
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.DateTime。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回默认值</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsDateTime (this string value, out DateTime result) {
        return DateTime.TryParse (value, out result);
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.DateTime。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回默认值</param>
    /// <param name="format">指定DateTime转换格式。</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsDateTime (this string value, out DateTime result, string format) {
        return DateTime.TryParseExact (value, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result);
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.DateTime。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回默认值</param>
    /// <param name="lowerlimit">实例值所在时间范围下限。</param>
    /// <param name="upperlimit">实例值所在时间范围上限。</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsDateTime (this string value, out DateTime result, DateTime lowerlimit, DateTime upperlimit) {
        return value.IsDateTime (out result) && lowerlimit <= result && result <= upperlimit;
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.DateTime。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回默认值</param>
    /// <param name="format">指定DateTime转换格式。</param>
    /// <param name="lowerlimit">实例值所在时间范围下限。</param>
    /// <param name="upperlimit">实例值所在时间范围上限。</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsDateTime (this string value, out DateTime result, string format, DateTime lowerlimit, DateTime upperlimit) {
        return value.IsDateTime (out result, format) && value.IsDateTime (out _, lowerlimit, upperlimit);
    }
    #endregion

    #region System.Decimal
    /// <summary>
    /// 将此实例的值转换为 System.Decimal。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="default_value">当此实例的值为 null 或无效的值时要返回的值。</param>
    /// <exception cref="System.FormatException">当此实例不是有效的System.Decimal且<paramref name="default_value"/>为null时引发异常。</exception>
    /// <returns>与当前实例值对应的 System.Decimal</returns>
    public static decimal ToDecimal (this string value, decimal? default_value = null) {
        return value.IsDecimal (out decimal result) ?
            result :
            (
                default_value.HasValue ?
                default_value.Value :
                throw new FormatException ("转换类型失败")
            );
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
        return value.ToString ().IsDecimal (out decimal result, digit);
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.Decimal。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="digit">指定Decimal小数位数。</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsDecimal (this string value, int? digit = null) {
        return value.IsDecimal (out _, digit);
    }

    /// <summary>
    /// 判断此实例的值是否可转换为 System.Decimal。
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="result">转换成功返回对应值，失败返回默认值</param>
    /// <param name="digit">指定Decimal小数位数。</param>
    /// <returns>可转换，则为 true；不可转换则为 false。</returns>
    public static bool IsDecimal (this string value, out decimal result, int? digit = null) {
        result = default (decimal);
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
            result = decimal.Parse (parse_result.ToString (DecimalDigit (digit.Value)));
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

    #region ToX2
    /// <summary>
    /// 将此实例值转换为16进制字符串
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns> 与此实例值字符串表示形式，以16进制字符串表示</returns>
    public static string ToX2String (this byte[] value) {
        StringBuilder x2 = new StringBuilder ();
        foreach (byte bt in value) {
            x2.Append (bt.ToX2String ());
        }
        return x2.ToString ();
    }

    /// <summary>
    /// 将此实例值转换为16进制字符串
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns> 与此实例值字符串表示形式，以16进制字符串表示</returns>
    public static string ToX2String (this byte value) => value.ToString ("x2");

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
    public static byte[] MD5Encryp (this string value) => ASCIIEncoding.UTF8.GetBytes (value).MD5Encryp ();

    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>MD5加密数据值</returns>
    public static byte[] MD5Encryp (this byte[] value) => MD5.Create ().ComputeHash (value);
    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>MD5加密数据值</returns>
    public static byte[] MD5Encryp (this Stream value) => value.ToByte ().MD5Encryp ();
    #endregion

    #region SHA1
    /// <summary>
    /// SHA1加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>SHA1加密数据值</returns>
    public static string SHA1Encryp (this string value) {
        return ASCIIEncoding.UTF8.GetBytes (value).SHA1Encryp ();
    }

    /// <summary>
    /// SHA1加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>SHA1加密数据值</returns>
    public static string SHA1Encryp (this byte[] value) {
        byte[] tmpHash = SHA1.Create ().ComputeHash (value);
        StringBuilder sha1 = new StringBuilder ();
        foreach (byte bt in tmpHash) {
            sha1.Append (bt.ToString ("x2"));
        }
        return sha1.ToString ();
    }

    /// <summary>
    /// SHA1加密
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>SHA1加密数据值</returns>
    public static string SHA1Encryp (this Stream value) {
        return value.ToByte ().SHA1Encryp ();
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

    /// <summary>
    /// 将此实例的值格式化为Number（逗号分隔）
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <exception cref="System.FormatException">当此实例不是有效的Sytem.Int32时引发异常。</exception>
    /// <returns>与当前实例值对应的Number</returns>
    public static string FormatNumber (this string value) {
        return value.ToInt ().FormatNumber ();
    }
    /// <summary>
    /// 将此实例的值格式化为Number（逗号分隔）
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>与当前实例值对应的Number</returns>
    public static string FormatNumber (this int value) {
        return value >= 1000 ? string.Format ("{0:0,0}", value) : value.ToString ();
    }

    #region 时间戳
    /// <summary>
    /// 将此实例值转换成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>返回时间戳值</returns>
    public static double ToTimeStamp (this DateTime value) {
        return value.Subtract (TimeZoneInfo.ConvertTime (new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local)).TotalSeconds;
    }

    /// <summary>
    /// 将此时间戳实例值转换成时间
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>返回时间戳对应DateTime</returns>
    public static DateTime ToTimeStamp (this double value) {
        return TimeZoneInfo.ConvertTime (new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds (value), TimeZoneInfo.Local);
    }

    /// <summary>
    /// 将此时间戳实例值转换成时间
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>返回时间戳对应DateTime</returns>
    public static DateTime ToTimeStamp (this long value) {
        return ((double) value).ToTimeStamp ();
    }

    /// <summary>
    /// 将此时间戳实例值转换成时间
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>返回时间戳对应DateTime</returns>
    public static DateTime ToTimeStamp (this string value) {
        return value.ToDouble ().ToTimeStamp ();
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this double value) {
        return value.IsTimeStamp (new DateTime (1753, 1, 1, 0, 0, 0), new DateTime (9999, 12, 31, 11, 59, 59));
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="timestamp">成功：转换成对应的时间，失败为默认时间</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this double value, out DateTime timestamp) {
        return value.IsTimeStamp (new DateTime (1753, 1, 1, 0, 0, 0), new DateTime (9999, 12, 31, 11, 59, 59), out timestamp);
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="start">时间范围起始</param>
    /// <param name="end">时间范围结束</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this double value, DateTime start, DateTime end) {
        double lowerlimit = start.ToTimeStamp (), upperlimit = end.ToTimeStamp ();
        return lowerlimit <= value && value <= upperlimit;
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="start">时间范围起始</param>
    /// <param name="end">时间范围结束</param>
    /// <param name="timestamp">成功：转换成对应的时间，失败为默认时间</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this double value, DateTime start, DateTime end, out DateTime timestamp) {
        bool result = value.IsTimeStamp (start, end);
        timestamp = result?value.ToTimeStamp () : default (DateTime);
        return result;
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this long value) {
        return value.IsTimeStamp (out _);
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="timestamp">成功：转换成对应的时间，失败为默认时间</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this long value, out DateTime timestamp) {
        return ((double) value).IsTimeStamp (out timestamp);
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="start">时间范围起始</param>
    /// <param name="end">时间范围结束</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this long value, DateTime start, DateTime end) {
        return value.IsTimeStamp (start, end, out _);
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="start">时间范围起始</param>
    /// <param name="end">时间范围结束</param>
    /// <param name="timestamp">成功：转换成对应的时间，失败为默认时间</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this long value, DateTime start, DateTime end, out DateTime timestamp) {
        return ((double) value).IsTimeStamp (start, end, out timestamp);
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this string value) {
        return value.IsTimeStamp (out _);
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="timestamp">成功：转换成对应的时间，失败为默认时间</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this string value, out DateTime timestamp) {
        timestamp = default (DateTime);
        return value.IsDouble (out double _double) && _double.IsTimeStamp (out timestamp);
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="start">时间范围起始</param>
    /// <param name="end">时间范围结束</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this string value, DateTime start, DateTime end) {
        return value.IsTimeStamp (start, end, out _);
    }

    /// <summary>
    /// 判断此实例是否可以转成成时间戳
    /// <para>时间戳:格林威治时间从1970年01月01日00时00分00秒起至现在的总秒数</para>
    /// </summary>
    /// <param name="value">当前实例值</param>
    /// <param name="start">时间范围起始</param>
    /// <param name="end">时间范围结束</param>
    /// <param name="timestamp">成功：转换成对应的时间，失败为默认时间</param>
    /// <returns>是否可以转换成时间戳（时间范围1753-1-1 0:0:0至9999-12-31 11:59:59）</returns>
    public static bool IsTimeStamp (this string value, DateTime start, DateTime end, out DateTime timestamp) {
        timestamp = default (DateTime);
        return value.IsDouble (out double _double) && _double.IsTimeStamp (start, end, out timestamp);
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

    /// <summary>
    /// 验证时间合法性
    /// </summary>
    /// <param name="time_start">起始时间</param>
    /// <param name="time_end">结束时间</param>
    /// <param name="time_type">时间格式</param>
    /// <returns>验证通过返回True，验证失败返回False</returns>
    public static bool ValidateDateTime (string time_start, string time_end, string time_type = "yyyy-MM-dd HH:mm:ss") {
        time_start = time_start ?? string.Empty;
        time_end = time_end ?? string.Empty;
        if (!time_start.IsDateTime (out DateTime time_starts, time_type) ||
            !time_end.IsDateTime (out DateTime time_ends, time_type) ||
            DateTime.Compare (time_starts, time_ends) > 0) {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 根据虚拟路径获取物理路径
    /// </summary>
    /// <param name="value">虚拟路径</param>
    /// <returns>物理路径</returns>
    public static string ToPhysicalPathBeginRoot (this string value) => $"{Directory.GetCurrentDirectory().TrimEnd('/', '\\')}{value?.TrimStart('/', '\\') ?? string.Empty}";
}