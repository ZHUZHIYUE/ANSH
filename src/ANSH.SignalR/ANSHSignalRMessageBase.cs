using System;

namespace ANSH.SignalR {
    /// <summary>
    /// SignalR消息基类
    /// </summary>
    public class ANSHSignalRMessageBase {

        /// <summary>
        /// 时间
        /// </summary>
        /// <value></value>
        public DateTime MessageTime { get; set; } = DateTime.Now;
    }
}