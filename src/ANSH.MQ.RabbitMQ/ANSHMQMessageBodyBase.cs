using System;
using Newtonsoft.Json;

namespace ANSH.MQ.RabbitMQ {
    /// <summary>
    /// 消息模型基类
    /// </summary>
    /// <typeparam name="TModel">消息正文模型</typeparam>
    [JsonObject (MemberSerialization.OptOut)]
    public abstract class ANSHMQMessageBodyBase<TModel> : ANSHMQMessageBase where TModel : new () {

        /// <summary>
        /// 消息正文
        /// </summary>
        /// <value></value>
        public TModel Body { get; set; } = new TModel ();
    }
}