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
}