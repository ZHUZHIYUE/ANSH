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
        /// 消息rootkey
        /// </summary>
        public abstract string RootKey { get; }

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