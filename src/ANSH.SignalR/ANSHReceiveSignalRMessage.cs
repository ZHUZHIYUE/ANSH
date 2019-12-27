namespace ANSH.SignalR {

    /// <summary>
    /// SignalR回调消息基类
    /// </summary>
    public class ANSHReceiveSignalRMessage : ANSHSignalRMessageBase {
        /// <summary>
        /// 发送人
        /// </summary>
        /// <value></value>
        public string InitiatorUser { get; set; }
    }
}