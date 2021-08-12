using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ANSH.MQ.RabbitMQ {
    /// <summary>
    /// 消息模型基类
    /// </summary>
    [JsonObject (MemberSerialization.OptOut)]
    public abstract class ANSHMQMessagePublishBase {
        /// <summary>
        /// 交换机名称
        /// </summary>
        public virtual string Exchange => "ansh.direct";

        /// <summary>
        /// 交换机类型
        /// </summary>
        public virtual string ExchangeType => "direct";

        /// <summary>
        /// 延迟交换机名称
        /// </summary>
        public virtual string ExchangeDelay => $"{Exchange}.delay";

        /// <summary>
        /// 延迟交换机类型
        /// </summary>
        public virtual string ExchangeTypeDelay => ExchangeType;

        /// <summary>
        /// 是否创建延迟队列
        /// </summary>
        /// <value></value>
        public virtual bool QueueDelayOpen => false;

        /// <summary>
        /// 延迟队列消息rootkey
        /// </summary>
        public virtual string RootKeyDelay => RootKey;

        /// <summary>
        /// 消息rootkey
        /// </summary>
        public abstract string RootKey { get; }

        /// <summary>
        /// 操作方式
        /// </summary>
        /// <value></value>
        [JsonConverter (typeof (StringEnumConverter))]
        public virtual OperatingMethod? Operating { get; set; } = OperatingMethod.Add;

        /// <summary>
        /// 消息有效处理时间单位毫秒
        /// </summary>
        /// <value></value>
        public virtual long? Expiration { get; set; }

        /// <summary>
        /// 是否为懒队列（先将消息保存到磁盘上，不放在内存中，当消费者开始消费的时候才加载到内存中）
        /// </summary>
        /// <value></value>
        public virtual bool IsLazy => false;

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