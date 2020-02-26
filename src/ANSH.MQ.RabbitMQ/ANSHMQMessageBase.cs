using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ANSH.MQ.RabbitMQ {
    /// <summary>
    /// 消息模型基类
    /// </summary>
    [JsonObject (MemberSerialization.OptOut)]
    public abstract class ANSHMQMessageBase : ANSHMQMessageRetrievingBase { }
}