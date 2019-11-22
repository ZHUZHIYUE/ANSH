using System;
using System.Collections.Generic;

namespace ANSH.Common.Comparer {
    /// <summary>
    /// 按照DateTime格式排序
    /// </summary>
    public class TimeComparer : IComparer<string> {
        int IComparer<string>.Compare (string left, string right) => left.ToDateTime (DateTime.Now.AddYears (20)).CompareTo (right.ToDateTime (DateTime.Now.AddYears (20)));
    }
}