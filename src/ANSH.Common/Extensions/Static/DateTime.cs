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
public static class ANSHCommonExtensionsDateTime {

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
    /// 按指定区域分区
    /// </summary>
    ///  <param name="beginTime">当前实例值</param>
    ///  <param name="timeSpanSave">指定时间区域，单位秒</param>
    ///  <param name="endTime">结束时间</param>
    /// <returns>返回指定实例与结束时间所在区域</returns>
    public static (DateTime, DateTime) [] ToTimePartition (this DateTime beginTime, int timeSpanSave, DateTime? endTime) {
        if (endTime.HasValue && endTime < beginTime) {
            throw new Exception ("结束时间必须大于当前实例值");
        }
        List < (DateTime, DateTime) > result = new List < (DateTime, DateTime) > ();
        int count = endTime.HasValue?(int) Math.Ceiling ((double) (endTime.Value - beginTime).TotalSeconds / timeSpanSave) : 1;
        count = count < 1 ? 1 : count;
        int remainder = (beginTime.Minute * 60 + beginTime.Second) % timeSpanSave;
        DateTime beginTimeItem = beginTime,
            endTimeItem = beginTime;
        for (int i = 1; i <= count; i++) {
            beginTimeItem = endTimeItem;
            beginTimeItem = beginTimeItem.AddSeconds (i == 1 ? -remainder : 0);
            endTimeItem = beginTimeItem;
            endTimeItem = endTimeItem.AddSeconds (timeSpanSave);

            result.Add ((beginTimeItem, endTimeItem));
        }
        return result.ToArray ();
    }

    /// <summary>
    /// 按指定区域分区
    /// </summary>
    ///  <param name="beginTime">当前实例值</param>
    ///  <param name="timeSpanSave">指定时间区域，单位分钟</param>
    /// <returns>返回指定实例所在区域</returns>
    public static (DateTime, DateTime) ToTimePartition (this DateTime beginTime, int timeSpanSave) => beginTime.ToTimePartition (timeSpanSave, null).FirstOrDefault ();
}