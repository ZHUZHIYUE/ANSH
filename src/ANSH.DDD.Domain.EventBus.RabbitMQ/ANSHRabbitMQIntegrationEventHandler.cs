using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ANSH.MQ.RabbitMQ;

namespace ANSH.DDD.Domain.EventBus.RabbitMQ {

    /// <summary>
    /// 事件
    /// </summary>
    /// <typeparam name="TANSHRibbitMQIntegrationEvent">事件类型</typeparam>
    /// <typeparam name="TANSHRibbitMQIntegrationEvent1">事件类型</typeparam>
    /// <typeparam name="TANSHRibbitMQIntegrationEvent2">事件类型</typeparam>
    /// <typeparam name="TANSHRibbitMQIntegrationEvent3">事件类型</typeparam>
    public abstract class ANSHRabbitMQIntegrationEventHandler<TANSHRibbitMQIntegrationEvent, TANSHRibbitMQIntegrationEvent1, TANSHRibbitMQIntegrationEvent2, TANSHRibbitMQIntegrationEvent3> : ANSHRabbitMQIntegrationEventHandler<TANSHRibbitMQIntegrationEvent, TANSHRibbitMQIntegrationEvent1, TANSHRibbitMQIntegrationEvent2>
        where TANSHRibbitMQIntegrationEvent : ANSHMQMessagePublishBase, new ()
    where TANSHRibbitMQIntegrationEvent1 : ANSHMQMessagePublishBase, new ()
    where TANSHRibbitMQIntegrationEvent2 : ANSHMQMessagePublishBase, new ()
    where TANSHRibbitMQIntegrationEvent3 : ANSHMQMessagePublishBase, new () {

        /// <summary>
        /// 消息rootkey
        /// </summary>
        public override string[] RootKey => base.RootKey.Concat (new TANSHRibbitMQIntegrationEvent1 ().RootKey).ToArray ();

    }

    /// <summary>
    /// 事件
    /// </summary>
    /// <typeparam name="TANSHRibbitMQIntegrationEvent">事件类型</typeparam>
    /// <typeparam name="TANSHRibbitMQIntegrationEvent1">事件类型</typeparam>
    /// <typeparam name="TANSHRibbitMQIntegrationEvent2">事件类型</typeparam>
    public abstract class ANSHRabbitMQIntegrationEventHandler<TANSHRibbitMQIntegrationEvent, TANSHRibbitMQIntegrationEvent1, TANSHRibbitMQIntegrationEvent2> : ANSHRabbitMQIntegrationEventHandler<TANSHRibbitMQIntegrationEvent, TANSHRibbitMQIntegrationEvent1>
        where TANSHRibbitMQIntegrationEvent : ANSHMQMessagePublishBase, new ()
    where TANSHRibbitMQIntegrationEvent1 : ANSHMQMessagePublishBase, new ()
    where TANSHRibbitMQIntegrationEvent2 : ANSHMQMessagePublishBase, new () {

        /// <summary>
        /// 消息rootkey
        /// </summary>
        public override string[] RootKey => base.RootKey.Concat (new TANSHRibbitMQIntegrationEvent1 ().RootKey).ToArray ();

    }

    /// <summary>
    /// 事件
    /// </summary>
    /// <typeparam name="TANSHRibbitMQIntegrationEvent">事件类型</typeparam>
    /// <typeparam name="TANSHRibbitMQIntegrationEvent1">事件类型</typeparam>
    public abstract class ANSHRabbitMQIntegrationEventHandler<TANSHRibbitMQIntegrationEvent, TANSHRibbitMQIntegrationEvent1> : ANSHRabbitMQIntegrationEventHandler<TANSHRibbitMQIntegrationEvent>
        where TANSHRibbitMQIntegrationEvent : ANSHMQMessagePublishBase, new ()
    where TANSHRibbitMQIntegrationEvent1 : ANSHMQMessagePublishBase, new () {

        /// <summary>
        /// 消息rootkey
        /// </summary>
        public override string[] RootKey => base.RootKey.Concat (new TANSHRibbitMQIntegrationEvent1 ().RootKey).ToArray ();

    }

    /// <summary>
    /// 事件
    /// </summary>
    /// <typeparam name="TANSHRibbitMQIntegrationEvent">事件类型</typeparam>
    public abstract class ANSHRabbitMQIntegrationEventHandler<TANSHRibbitMQIntegrationEvent> : ANSHMQMessageRetrievingBase
    where TANSHRibbitMQIntegrationEvent : ANSHMQMessagePublishBase, new () {

        /// <summary>
        /// 消息rootkey
        /// </summary>
        public override string[] RootKey => new TANSHRibbitMQIntegrationEvent ().RootKey;

    }
}