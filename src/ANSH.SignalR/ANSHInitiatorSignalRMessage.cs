namespace ANSH.SignalR {
    /// <summary>
    /// SignalR发送消息基类
    /// </summary>
    public class ANSHInitiatorSignalRMessage : ANSHSignalRMessageBase {
        /// <summary>
        /// 接收人
        /// </summary>
        /// <value></value>
        public string ReceivedUser { get; set; }
    }
}