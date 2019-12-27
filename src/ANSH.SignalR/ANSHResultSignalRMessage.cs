namespace ANSH.SignalR {

    /// <summary>
    /// SignalR返回消息基类
    /// </summary>
    public class ANSHResultSignalRMessage : ANSHSignalRMessageBase {
        /// <summary>
        /// 消息代码
        /// </summary>
        /// <value></value>
        public int MsgCode { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        /// <value></value>
        public string Msg { get; set; } = "SUCCESS";
    }
}