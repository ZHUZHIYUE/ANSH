using ANSH.MQ.RabbitMQ;

namespace ANSH.DDD.Domain.EventBus.RabbitMQ {
    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <typeparam name="TModel">数据模型</typeparam>
    public abstract class ANSHRibbitMQIntegrationEvent<TModel> : ANSHMQMessagePublishBase {

        /// <summary>
        /// 消息正文
        /// </summary>
        /// <value></value>
        public TModel Body { get; set; } = default (TModel);
    }
}