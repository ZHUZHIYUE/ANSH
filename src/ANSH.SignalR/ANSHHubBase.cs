using Microsoft.AspNetCore.SignalR;

namespace ANSH.SignalR {
    /// <summary>
    /// SignalR服务基类
    /// </summary>
    /// <typeparam name="TANSHChatClientBase">客户端回调方法</typeparam>
    public class ANSHHubBase<TANSHChatClientBase> : Hub<TANSHChatClientBase> where TANSHChatClientBase : class, IANSHSignalRClientBase {

    }
}