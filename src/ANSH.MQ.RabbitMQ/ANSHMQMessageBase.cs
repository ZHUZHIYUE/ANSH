using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ANSH.MQ.RabbitMQ {
    /// <summary>
    /// 消息模型基类
    /// </summary>
    [JsonObject (MemberSerialization.OptOut)]
    public abstract class ANSHMQMessageBase {
        /// <summary>
        /// 交换机名称
        /// </summary>
        public virtual string Exchange => "ansh.direct";

        /// <summary>
        /// 交换机类型
        /// </summary>
        public virtual string ExchangeType => "direct";
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
        /// 消息rootkey
        /// </summary>
        public abstract string RootKey { get; }

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
        /// 操作方式
        /// </summary>
        /// <value></value>
        [JsonConverter (typeof (StringEnumConverter))]
        public virtual OperatingMethod? Operating { get; set; } = OperatingMethod.Add;

    }

    /// <summary>
    /// 操作方式
    /// </summary>
    public enum OperatingMethod {
        /// <summary>
        /// 添加
        /// </summary>
        Add,
        /// <summary>
        /// 修改
        /// </summary>
        Update,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 获取
        /// </summary>
        Get,
        /// <summary>
        /// 其他
        /// </summary>
        Other
    }
}