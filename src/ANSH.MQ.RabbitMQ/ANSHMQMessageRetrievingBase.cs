using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ANSH.MQ.RabbitMQ {
    /// <summary>
    /// 消息模型基类
    /// </summary>
    [JsonObject (MemberSerialization.OptOut)]
    public abstract class ANSHMQMessageRetrievingBase : ANSHMQMessagePublishBase {

        /// <summary>
        /// 死信交换机名称
        /// </summary>
        public virtual string ExchangeDX => $"{Exchange}.dx";

        /// <summary>
        /// 死信交换机类型
        /// </summary>
        public virtual string ExchangeTypeDX => ExchangeType;

        /// <summary>
        /// 队列名称
        /// </summary>
        public abstract string Queue { get; }

        /// <summary>
        /// 是否创建死信队列
        /// </summary>
        /// <value></value>
        public virtual bool QueueDxOpen => true;

        /// <summary>
        /// 死信队列名称
        /// </summary>
        /// <value></value>
        public virtual string QueueDX => $"{Queue}.dx";

        /// <summary>
        /// 延迟队列名称
        /// </summary>
        /// <value></value>
        public virtual string QueueDelay => $"{Queue}.delay";

    }
}