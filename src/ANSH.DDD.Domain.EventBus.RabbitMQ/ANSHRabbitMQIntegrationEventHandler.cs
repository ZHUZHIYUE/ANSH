using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ANSH.MQ.RabbitMQ;

namespace ANSH.DDD.Domain.EventBus.RabbitMQ {
    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <typeparam name="TANSHRibbitMQIntegrationEvent">事件类型</typeparam>
    public abstract class ANSHRabbitMQIntegrationEventHandler<TANSHRibbitMQIntegrationEvent> : ANSHMQMessageRetrievingBase
    where TANSHRibbitMQIntegrationEvent : ANSHMQMessagePublishBase, new () {

        /// <summary>
        /// 消息rootkey
        /// </summary>
        public override string RootKey => new TANSHRibbitMQIntegrationEvent ().RootKey;

    }
}